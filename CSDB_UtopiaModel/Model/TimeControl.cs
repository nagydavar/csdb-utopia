using System.Runtime.CompilerServices;

namespace CSDB_UtopiaModel.Model;

public class TimeControl
{
    #region Exceptions

    public class AlreadyAtMaxSpeedException : InvalidOperationException
    {
        public AlreadyAtMaxSpeedException() : base("The timer is already at maximum speed.")
        {
        }

        public AlreadyAtMaxSpeedException(string? message) : base(message)
        {
        }

        public AlreadyAtMaxSpeedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }

    public class AlreadyAtMinSpeedException : InvalidOperationException
    {
        public AlreadyAtMinSpeedException() : base("The timer is already at minimum speed.")
        {
        }

        public AlreadyAtMinSpeedException(string? message) : base(message)
        {
        }

        public AlreadyAtMinSpeedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }

    public class AlreadySubscribedException : InvalidOperationException
    {
        public AlreadySubscribedException() : base("The class has already subscribed before.")
        {
        }

        public AlreadySubscribedException(string? message) : base(message)
        {
        }

        public AlreadySubscribedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }

    public class AlreadySubscribedWithAnotherValueException : AlreadySubscribedException
    {
        public AlreadySubscribedWithAnotherValueException() : base(
            "The class has already subscribed with another value before.")
        {
        }

        public AlreadySubscribedWithAnotherValueException(string? message) : base(message)
        {
        }

        public AlreadySubscribedWithAnotherValueException(string? message, Exception? innerException) : base(message,
            innerException)
        {
        }
    }

    public class NotSubscribedException : InvalidOperationException
    {
        public NotSubscribedException() : base("The class has not subscribed, therefore cannot be removed.")
        {
        }

        public NotSubscribedException(string? message) : base(message)
        {
        }

        public NotSubscribedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }

    #endregion

    #region Fields

    private static TimeControl? _instance;

    private int _index;

    /// <summary>
    /// Least Common Multiple
    /// </summary>
    private int _lcm;

    private readonly System.Timers.Timer _timer;

    private readonly Dictionary<ITickable, int> _subscriptions = new();

    #endregion

    #region Properties

    public TimerSpeed Speed { get; private set; }

    public bool IsStopped => _timer.Enabled;

    #endregion

    #region Constructor

    protected TimeControl()
    {
        Speed = TimerSpeed.Normal;
        _timer = new(IntervalFromTimerSpeed(Speed))
        {
            AutoReset = true,
        };

        _lcm = 0;
        _index = 0;

        _timer.Elapsed += async (_, _) =>
        {
            //Biztonsági ellenõrzés, ha nincs feliratkozó vagy az LCM 0, ne csináljunk semmit
            if (_lcm <= 0 || _subscriptions.Count == 0) return;

            List<Task> tasks = new(_subscriptions.Count);

            //Index léptetése 1-tõl LCM-ig pörögjön (így az index sosem lesz 0)
            // Az (index % lcm) 0-tól (lcm-1)-ig adna értéket, ezért adunk hozzá 1-et.
            _index = (_index % _lcm) + 1;

            foreach (var subscription in _subscriptions)
            {
                //akkor tickeljen az objektum, ha az aktuális index 
                // osztható az objektum saját feliratkozási értékével.
                if (_index % subscription.Value == 0)
                {
                    tasks.Add(subscription.Key.Tick());
                }
            }

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
        };
    }

    #endregion

    #region Non-public methods

    private static int IntervalFromTimerSpeed(TimerSpeed speed) => speed switch
    {
        TimerSpeed.Normal => 500,
        TimerSpeed.Fast => 250,
        TimerSpeed.SpeedOfLight => 125,
        _ => -1 // can also be an exception
    };

    protected static TimeControl Unsubscribe(TimeControl timeControl, ITickable key, int? value)
    {
        if (!timeControl._subscriptions.TryGetValue(key, out int actualValue))
            throw new NotSubscribedException();

        if (value.HasValue && value.Value != actualValue)
            throw new AlreadySubscribedWithAnotherValueException();

        timeControl._subscriptions.Remove(key);

        // handle LCM

        return timeControl;
    }

    protected static int LcmOf(IEnumerable<int> numbers)
    {
        int? prev = null;

        foreach (int number in numbers)
            prev = prev.HasValue ? LcmOf(prev.Value, number) : number;

        return prev ?? throw new Exception();
    }

    protected static int LcmOf(int a, int b) => a / GcdOf(a, b) * b;

    protected static int GcdOf(int a, int b)
    {
        while (b != 0)
        {
            int tmp = b;
            b = a % b;
            a = tmp;
        }

        return a;
    }

    #endregion

    #region Public methods

    #region Operators

    public static TimeControl operator !(TimeControl timeControl)
    {
        if (timeControl.IsStopped)
            timeControl.Resume();
        else
            timeControl.Pause();

        return timeControl;
    }

    /// <summary>
    /// Subscribes an ITickable class 
    /// </summary>
    public static TimeControl operator +(TimeControl timeControl, (ITickable Key, int Value) subscriber)
    {
        if (subscriber.Value <= 0)
            throw new ArgumentException("The subscriber's value must be positive.", nameof(subscriber.Value));

        // ReferenceContains
        foreach (var subscription in (timeControl._subscriptions))
            if (ReferenceEquals(subscription.Key, subscriber.Key))
                throw new AlreadySubscribedException();

        if (timeControl._lcm % subscriber.Value != 0)
            timeControl._lcm = LcmOf(timeControl._subscriptions.Values);

        timeControl._subscriptions.Add(subscriber.Key, subscriber.Value);

        // Újraszámoljuk az LCM-et minden feliratkozásnál
        timeControl._lcm = LcmOf(timeControl._subscriptions.Values);

        return timeControl;
    }

    /// <summary>
    /// Unsubscribes an ITickable class 
    /// </summary>
    public static TimeControl operator -(TimeControl timeControl, (ITickable Key, int Value) unsubscriber) =>
        Unsubscribe(timeControl, unsubscriber.Key, unsubscriber.Value);

    public static TimeControl operator -(TimeControl timeControl, ITickable key) => Unsubscribe(timeControl, key, null);

    /// <summary>
    /// Increases the timer's speed
    /// </summary>
    public static TimeControl operator ++(TimeControl timeControl)
    {
        if (timeControl.Speed == TimerSpeed.SpeedOfLight)
            throw new AlreadyAtMaxSpeedException();

        timeControl._timer.Interval = IntervalFromTimerSpeed(++timeControl.Speed);

        return timeControl;
    }

    /// <summary>
    /// Decreases the timer's speed
    /// </summary>
    public static TimeControl operator --(TimeControl timeControl)
    {
        if (timeControl.Speed == TimerSpeed.Normal)
            throw new AlreadyAtMinSpeedException();

        timeControl._timer.Interval = IntervalFromTimerSpeed(--timeControl.Speed);

        return timeControl;
    }

    #endregion

    public static TimeControl Instance() => _instance ??= new();

    public void Pause() => _timer.Start();
    public void Resume() => _timer.Stop();

    public void ChangeValue(ITickable key, int newValue) => _subscriptions[key] = newValue;

    #endregion
}
using System.Runtime.CompilerServices;

namespace CSDB_UtopiaModel.Model;

public class TimeControl
{
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

    private static TimeControl? _instance;

    private readonly System.Timers.Timer _timer;

    private int _index;

    // private int GCD;
    /// <summary>
    /// Least Common Multiple
    /// </summary>
    private int _LCM;

    private Dictionary<ITickable, int> _subscriptions = new();

    public TimerSpeed Speed { get; private set; }

    public bool IsStopped => _timer.Enabled;

    protected TimeControl()
    {
        Speed = TimerSpeed.Normal;
        _timer = new(IntervalFromTimerSpeed(Speed))
        {
            AutoReset = true,
        };
        _LCM = 0;
        _index = 0;

        _timer.Elapsed += async (_, _) =>
        {
            List<Task> tasks = new(_subscriptions.Count);

            _index = ++_index % _LCM; // hope it works...

            foreach (var subscription in _subscriptions)
            {
                if (subscription.Value % _index == 0)
                    tasks.Add(subscription.Key.Tick());
            }

            await Task.WhenAll(tasks);
        };
    }

    public static TimeControl Instance() => _instance ??= new();

    private static int IntervalFromTimerSpeed(TimerSpeed speed) => speed switch
    {
        TimerSpeed.Normal => 500,
        TimerSpeed.Fast => 250,
        TimerSpeed.SpeedOfLight => 125,
        _ => -1 // can also be an exception
    };

    protected static TimeControl Unsubscribe(TimeControl timeControl, ITickable key, int? value)
    {
        if (value.HasValue && timeControl._subscriptions[key] != value.Value) // !!!!
            throw new AlreadySubscribedWithAnotherValueException();

        if (!timeControl._subscriptions.Remove(key))
            throw new NotSubscribedException(); // is this necessary?

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

        if (timeControl._LCM % subscriber.Value != 0)
            timeControl._LCM = LcmOf(timeControl._subscriptions.Values);

        timeControl._subscriptions.Add(subscriber.Key, subscriber.Value);

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

    public void Pause() => _timer.Start();
    public void Resume() => _timer.Stop();

    public void ChangeValue(ITickable _) => throw new NotImplementedException();
}
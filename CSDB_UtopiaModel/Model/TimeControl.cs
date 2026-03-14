using System.Runtime.CompilerServices;

namespace CSDB_UtopiaModel.Model;

public class TimeControl
{
    public class AlreadyAtMaxSpeedException : Exception
    {
        public AlreadyAtMaxSpeedException() : base("The timer is already at maximum speed.")
        {
        }

        public AlreadyAtMaxSpeedException(string message) : base(message)
        {
        }

        public AlreadyAtMaxSpeedException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class AlreadyAtMinSpeedException : Exception
    {
        public AlreadyAtMinSpeedException() : base("The timer is already at minimum speed.")
        {
        }

        public AlreadyAtMinSpeedException(string message) : base(message)
        {
        }

        public AlreadyAtMinSpeedException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    private static TimeControl? _instance;

    private readonly System.Timers.Timer _timer;

    private int index;

    // private int GCD;
    /// <summary>
    /// Least Common Multiple
    /// </summary>
    private int LCM;

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
        LCM = 0;
        index = 0;

        _timer.Elapsed += async (_, _) =>
        {
            List<Task> tasks = new(_subscriptions.Count);

            index = ++index % LCM; // hope it works...

            foreach (var subscription in _subscriptions)
            {
                if (subscription.Value % index == 0)
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

    /// <summary>
    /// Subscribes an ITickable class 
    /// </summary>
    public static TimeControl operator +(TimeControl timeControl, (int, ITickable) subscriber)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Unsubscribes an ITickable class 
    /// </summary>
    public static TimeControl operator -(TimeControl timeControl, (int, ITickable) subscriber)
    {
        throw new NotImplementedException();
    }
    
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
}
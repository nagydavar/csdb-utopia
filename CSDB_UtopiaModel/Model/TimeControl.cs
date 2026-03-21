using System.Runtime.CompilerServices;

namespace CSDB_UtopiaModel.Model;

public class TimeControl
{
    private static TimeControl? instance;
    private TimeControl() { }
    public static TimeControl Instance()
    {
        if (instance is null)
            instance = new TimeControl();
        return instance;
    }
    private bool isStopped;
    private Timer timer;
    public int index;
    public int GCD;
    public int step;
    public Dictionary<int, Ticker> Subscribed;
    public void Subscribe(int, Tickable);
    public TimerSpeed GetSpeed;
    public void Pause();
    public void Resume();
    public void Plus();
    public void Minus();

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
namespace CSDB_UtopiaModel.Model;

class TimeControl
{
<<<<<<< HEAD
    private TimeControl? instance;
    private TimeControl() { }
    public TimeControl Instance()
=======
    private static TimeControl? instance;

    private TimeControl()
    {
    }

    public static TimeControl Instance()
>>>>>>> 29c97f5 (Small changes in timer-related classes)
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


}
namespace CSDB_UtopiaModel.Model;

class TimeControl
{
    private static TimeControl? instance;

    private TimeControl()
    {
    }

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


}
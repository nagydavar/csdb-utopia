namespace CSDB_UtopiaModel.Model;

public class TimeControl
{
    private static TimeControl? _instance;
    
    private System.Timers.Timer _timer;
    private int index;
    private int GCD;
    private int step;
    private Dictionary<int, ITickable> _subscriptions = new();
    
    public TimerSpeed Speed { get; private set; }
    public bool IsStopped => _timer.Enabled;

    protected TimeControl()
    {
        
    }

    public static TimeControl Instance() => _instance ??= new();
    
    public void Subscribe(int _, ITickable _)=> throw new NotImplementedException();

    public void Pause() => throw new NotImplementedException();
    public void Resume() => throw new NotImplementedException();
    public void Plus() => throw new NotImplementedException();
    public void Minus() => throw new NotImplementedException();
}
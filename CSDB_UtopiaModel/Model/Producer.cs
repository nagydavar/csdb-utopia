using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Producer : Building, ITickable
{
    protected int yield;
    private readonly Model _model;
    protected Stop? stop;
    protected Field? location;
    
    public abstract int RequiredAmount { get; }
    
    public abstract int ProducedAmount { get; }

    protected Producer(Field f, int yield, Model model) : base(f)
    {
        this.yield = yield;
        _model = model;

        var timeControl = TimeControl.Instance();

        timeControl += (this, yield);
    }
    public Stop ConnectsTo { get; set; }
    public abstract IResource Produce();

    public abstract IResource Require();

    public Task Tick()
    {
        if (Owner is null)
            return Task.CompletedTask;
        
        if (stop is null)
        {
            for (int i = 0; i <= area.Width; i++)
            {
                for (int k = 0; k <= area.Height; k++)
                {
                    Coordinate c;

                    try
                    {
                       c = new(Owner.Coordinates.X + i, Owner.Coordinates.Y - k);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    
                    Field? f=null;

                    foreach (var p in c.GetAllNeighbors())
                    {
                        try
                        {
                            f = _model.GetField(p.Value);
                        }
                        catch (Exception)
                        {
                        }   
                    }

                    if (f?.Buildable is not Stop stop) continue;
                    
                    this.stop = stop;
                    i = k = int.MaxValue;
                    break;
                }
            }

            if (stop is /*still*/ null)
                return Task.CompletedTask;
        }

        if (location is null)
        {
            for (int i = 0; i <= area.Width; i++)
            {
                for (int k = 0; k <= area.Height; k++)
                {
                    Coordinate c;

                    try
                    {
                       c = new(Owner.Coordinates.X + i, Owner.Coordinates.Y - k);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    
                    Field? f = null;
                    
                    try
                    {
                        f = _model.GetField(c);
                    }
                    catch (Exception)
                    {
                    }

                    if (f?.Resource.GetType() == Require().GetType() && f.DepletionLevel > 0)
                        location = f;
                }
            }
            
            if(location is /*still*/ null) return Task.CompletedTask;
        }

        int ra = RequiredAmount;
        
        if (stop[Require()] < ra) return Task.CompletedTask;

        stop.Unload(Require(), ra);
        
        int pa = ProducedAmount;
        pa *= (int)Math.Round(Math.Sqrt(Math.Abs(_model.Mood))) * Math.Sign(_model.Mood);
        location.DepletionLevel -= pa;
        stop.Load(Produce(), pa);
        
        return Task.CompletedTask;
    }
}
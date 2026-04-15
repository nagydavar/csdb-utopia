using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Road : Buildable, INavigable
{
    private IVehicle? _leftSide;
    private IVehicle?  _rightSide;
    
    public int MaxSpeed { get; set; }

    public IVehicle? LeftSide
    {
        get => _leftSide;
        set
        {
            if (_leftSide == value) return;
            _leftSide = value;
            if (value is null)
                Freed?.Invoke(this, new DirectionEventArgs(LEFT.Instance()));
        }
    }

    public IVehicle? RightSide
    {
        get => _rightSide;
        set
        {
            if (_rightSide == value) return;
            _rightSide = value;
            if (value is null)
                Freed?.Invoke(this, new DirectionEventArgs(RIGHT.Instance()));
        }
    }

    // public IVehicle? LeftSide => new Bus();
    // public Vehicle<Resource>? RightSide => new Bus();

    public HashSet<Section> Sections { get; set; } = new();

    public IDirection Direction { get; set; }

    public EventHandler<DirectionEventArgs>? Freed;

    public Road(Field f, int maxSpeed, IDirection d) : base(f)
    {
        MaxSpeed = maxSpeed;
        Direction = d;
    }

    public bool IsFree(IDirection dir)
    {
        if (Direction is VerticalDirection)
        {
            if (dir is not VerticalDirection)
                throw new Exception();
            
            return dir == DOWN.Instance() ? LeftSide is null : RightSide is null;
        }
        else
        {
            if (dir is not HorizontalDirection)
                throw new Exception();
            
            return dir == RIGHT.Instance() ? LeftSide is null : RightSide is null;
        }
    }

    public void MoveTo() => throw new NotImplementedException();
}
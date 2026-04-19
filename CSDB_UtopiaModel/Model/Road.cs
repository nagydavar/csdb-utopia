using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Road : Buildable, INavigable
{
    private IVehicle? _leftSide;
    private IVehicle? _rightSide;

    public int MaxSpeed { get; set; }

    public IVehicle? LeftSide
    {
        get => _leftSide;
        set
        {
            if (ReferenceEquals(_leftSide, value)) return;
            if (_leftSide is not null && value is not null)
                throw new InvalidOperationException("Left side of the road is occupied");

            _leftSide = value;
            if (value is null)
                Freed?.Invoke(this, new DirectionEventArgs(Left.Instance()));
        }
    }

    public IVehicle? RightSide
    {
        get => _rightSide;
        set
        {
            if (ReferenceEquals(_rightSide, value)) return;
            if (_rightSide is not null && value is not null)
                throw new InvalidOperationException("Right side of the road is occupied");

            _rightSide = value;
            if (value is null)
                Freed?.Invoke(this, new DirectionEventArgs(Right.Instance()));
        }
    }

    // public IVehicle? LeftSide => new Bus();
    // public Vehicle<Resource>? RightSide => new Bus();

    //public HashSet<Section> Sections { get; set; } = new();

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

            return dir == Down.Instance() ? LeftSide is null : RightSide is null;
        }
        else
        {
            if (dir is not IHorizontalDirection)
                throw new Exception();

            return dir == Right.Instance() ? LeftSide is null : RightSide is null;
        }
    }

    public void MoveTo() => throw new NotImplementedException();
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Road : INavigable
{
    private IVehicle? _leftSide;
    private IVehicle? _rightSide;

    public int MaxSpeed { get; protected set; }

    public int Quadrant { get; internal set; } = 0;

    public bool IsCurved { get; internal set; } = false;
    
    public IDirection Direction { get; internal set; }

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
    
    public override int placementCost => 100;

    public EventHandler<DirectionEventArgs>? Freed;

    public Road(Field f, int maxSpeed, IDirection d) : base(f)
    {
        MaxSpeed = maxSpeed;
        Direction = d;

        area = (1, 1);
    }
    public bool IsRightSideFree() => RightSide is null;
    public bool IsLeftSideFree() => LeftSide is null;
    

    public bool IsFree(IDirection dir)
    {
        bool isRs = IsRightSide(dir);
        return (isRs && IsRightSideFree()) || (!isRs && IsLeftSideFree());
    }

    public override void Leave(IVehicle vehicle)
    {
        if (LeftSide == vehicle)
            LeftSide = null;
        if (RightSide == vehicle)
            RightSide = null;
    }

    public override bool TryMoveTo(IDirection dir, IVehicle vehicle)
    {
        try
        {
            if (IsRightSide(dir)) RightSide = vehicle;
            else LeftSide = vehicle;
        }
        catch (InvalidOperationException e)
        {
            return false;
        }
        return true;
    }

    public static bool IsRightSide(IDirection d)
    {
        
        if (d == Up.Instance()) return true;
        if (d == Down.Instance()) return false;
        if (d == Left.Instance()) return true;
        return false;
    }

}
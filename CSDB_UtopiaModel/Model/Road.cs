using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Road : Buildable, Navigable
{
    public int MaxSpeed { get; set; }

    public IVehicle? LeftSide { get; set; }
    public Vehicle<Resource>? RightSide { get; set; }
    
    // public IVehicle? LeftSide => new Bus();
    // public Vehicle<Resource>? RightSide => new Bus();
    
    public HashSet<Section> Sections { get; set; }

    public Direction Direction { get; set; }

    public EventHandler<DirectionEventArgs>? Freed;

    public Road(Field f, int maxSpeed, Direction d) : base(f)
    {
        MaxSpeed = maxSpeed;
        Direction = d;
    }

    public bool IsFree(Direction _) => throw new NotImplementedException();

    public void MoveTo() => throw new NotImplementedException();
}
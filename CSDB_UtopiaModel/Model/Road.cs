using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class Road : Buildable, Navigable
{
    public int MaxSpeed { get; set; }

    public IVehicle? LeftSide { get; set; }

    public IVehicle? RightSide { get; set; }

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
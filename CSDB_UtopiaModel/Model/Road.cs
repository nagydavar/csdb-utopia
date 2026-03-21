using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Road : Buildable, Navigable
{
    public int MaxSpeed { get; set; }

    public IVehicle? LeftSide { get; set; }

    public IVehicle? RightSide { get; set; }

    public HashSet<Section> Sections { get; set; }

    public Directions Direction { get; set; }

    public EventHandler<DirectionEventArgs>? Freed;

    public Road(Coordinate _, int maxSpeed, Direction _) => throw new NotImplementedException();

    public bool IsFree(Direction _) => throw new NotImplementedException();

    public void MoveTo() => throw new NotImplementedException();
}
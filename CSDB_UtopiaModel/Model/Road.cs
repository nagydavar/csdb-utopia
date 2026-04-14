using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Road : Buildable, Navigable
{
    public int MaxSpeed { get; set; }

    public IVehicle? LeftSide { get; set; }
    public Vehicle<IResource>? RightSide { get; set; }
    
    // public IVehicle? LeftSide => new Bus();
    // public Vehicle<Resource>? RightSide => new Bus();
    
    public HashSet<Section> Sections { get; set; }

    public IDirection IDirection { get; set; }

    public EventHandler<DirectionEventArgs>? Freed;

    public Road(Field f, int maxSpeed, IDirection d) : base(f)
    {
        MaxSpeed = maxSpeed;
        IDirection = d;
    }

    public bool IsFree(IDirection _) => throw new NotImplementedException();

    public void MoveTo() => throw new NotImplementedException();
}
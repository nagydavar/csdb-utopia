using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public interface IVehicle : ITickable, Buyable
{
    // TODO

    // A ford�t� csak akkor engedi a road.RightSide.Intention h�v�st,
    // ha ez szerepel itt:
    GoingIntention Intention { get; }
    public IDirection CurrentDirection { get; protected set; }

    public abstract void AssignNewPath(Coordinate[] stops);
    public abstract Task Tick();
    
    public abstract int placementCost { get; }

    public int Speed { get; }
    public int Capacity { get; }
    int MaintenanceCost { get; }
    public int carriedAmount { get; protected set; }
    
}
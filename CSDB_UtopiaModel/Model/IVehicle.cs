using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public interface IVehicle : ITickable, Buyable
{
    Coordinate Position { get; }
    GoingIntention Intention { get; }
    public IDirection CurrentDirection { get; protected set; }

    public abstract void AssignNewPath(Coordinate[] stops);

    public abstract bool CanCarry(IResource resource);

    public int Speed { get; }
    public int Capacity { get; }
    int MaintenanceCost { get; }
    public int carriedAmount { get; protected set; }

    string CarriedResourceName { get; } // Ezt fogjuk megjeleníteni

    int TraveledSinceBought { get; }
}
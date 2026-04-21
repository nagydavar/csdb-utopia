using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public interface IVehicle : ITickable, Buyable
{
    // TODO

    // A ford�t� csak akkor engedi a road.RightSide.Intention h�v�st,
    // ha ez szerepel itt:

    Coordinate Position { get; }
    GoingIntention Intention { get; }
    public IDirection CurrentDirection { get; protected set; }

    public void AssignNewPath(Coordinate[] stops);

    int Speed { get; }
    int Capacity { get; }
    int MaintenanceCost { get; }

    int TraveledSinceBought { get; }
}
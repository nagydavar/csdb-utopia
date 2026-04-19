using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public interface IVehicle : ITickable, Buyable
{
    // TODO

    // A fordító csak akkor engedi a road.RightSide.Intention hívást,
    // ha ez szerepel itt:
    GoingIntention Intention { get; }
    public IDirection CurrentDirection { get; protected set; }
}
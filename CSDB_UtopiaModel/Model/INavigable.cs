using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public interface INavigable
{
    bool TryMoveTo(IDirection dir, IVehicle vehicle);
    void Leave(IVehicle vehicle);
}
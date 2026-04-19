using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public interface INavigable
{
    void MoveTo(IDirection dir, IVehicle vehicle);
    void Leave(IVehicle vehicle);
}
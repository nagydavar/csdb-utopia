using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class INavigable: Buildable
{
    public abstract bool TryMoveTo(IDirection dir, IVehicle vehicle);
    public abstract void Leave(IVehicle vehicle);
    public INavigable(Field f):base(f)
    {}
}
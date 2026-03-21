using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class PassengerVehicle : Vehicle<HumanResource>
{
    public PassengerVehicle() : base(default!, default, default, default, default!)
    {
    }
}
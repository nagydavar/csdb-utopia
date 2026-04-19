using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class PassengerVehicle : Vehicle<HumanResource>
{
    public PassengerVehicle(Map map, Model m, Coordinate start, Coordinate end) : base(map,m, start, end)
    {
    }
}
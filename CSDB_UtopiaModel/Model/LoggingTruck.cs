using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class LoggingTruck : GoodsVehicle<Wood>
{
    public LoggingTruck(Map map, Model m, Coordinate start, Coordinate end) : base(map,m, start, end)
    {
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
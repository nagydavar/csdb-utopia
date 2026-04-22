using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class LoggingTruck : GoodsVehicle<Wood>
{
    public override int placementCost { get; } = 250;
    
    public LoggingTruck(Map map, Model m) : base(map,m)
    {
        maintenanceCost = 70;
        speed = 70;
        capacity = 30;
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
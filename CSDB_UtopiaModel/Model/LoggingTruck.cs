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
    public override bool CanCarry(IResource resource)
    {
        return resource is Wood;
    }
}
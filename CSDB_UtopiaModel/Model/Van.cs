using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class Van<TIndustrialResource> : GoodsVehicle<TIndustrialResource> where TIndustrialResource : IIndustrialResource
{
    public override int placementCost { get; } = 200;
    
    public Van(Map map, Model m, Coordinate start, Coordinate end) : base(map,m)
    {
        maintenanceCost = 50;
        speed = 90;
        capacity = 20;
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
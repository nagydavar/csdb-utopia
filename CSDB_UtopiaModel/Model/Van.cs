using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class Van<TIndustrialResource> : GoodsVehicle<TIndustrialResource> where TIndustrialResource : IIndustrialResource
{
    public Van(Map map, Model m, Coordinate start, Coordinate end) : base(map,m)
    {
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
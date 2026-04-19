using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class DumpTruck<TIndustrialResource> : GoodsVehicle<TIndustrialResource>
    where TIndustrialResource : IIndustrialResource
{
    public DumpTruck(Map map, Model m, Coordinate start, Coordinate end) : base(map,m, start, end)
    {
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
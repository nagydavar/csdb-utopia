using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class IndustrialDumpTruck<TIndustrialResource> : GoodsVehicle<TIndustrialResource>
    where TIndustrialResource : IIndustrialResource
{
    public override int placementCost { get; } = 300;
    
    public IndustrialDumpTruck(Map map, Model m) : base(map,m)
    {
        maintenanceCost = 100;
        speed = 70;
        capacity = 50;
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}

public class DumpTruck : IndustrialDumpTruck<IIndustrialResource> { public DumpTruck(Map m, Model mo) : base(m, mo) { } }
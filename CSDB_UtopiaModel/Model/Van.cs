using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class IndustrialVan<TIndustrialResource> : GoodsVehicle<TIndustrialResource> where TIndustrialResource : IIndustrialResource
{
    public override int placementCost { get; } = 200;
    
    public IndustrialVan(Map map, Model m) : base(map,m)
    {
        maintenanceCost = 50;
        speed = 90;
        capacity = 20;
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}

public class Van : IndustrialVan<IIndustrialResource> { public Van(Map m, Model mo) : base(m, mo) { } }
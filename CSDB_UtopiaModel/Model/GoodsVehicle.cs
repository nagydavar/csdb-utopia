using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class GoodsVehicle<TIndustrialResource> : Vehicle<TIndustrialResource>
    where TIndustrialResource : IIndustrialResource
{
    public GoodsVehicle(Map map, Model m) : base(map,m)
    {
    }
}
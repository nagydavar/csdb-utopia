using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class Van<TIndustrialResource> : GoodsVehicle<TIndustrialResource> where TIndustrialResource : IIndustrialResource
{
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class DumpTruck<R> where R : Resource
{
    private override int capacity;
    private override int maintenanceCost;
    private override int speed;
};

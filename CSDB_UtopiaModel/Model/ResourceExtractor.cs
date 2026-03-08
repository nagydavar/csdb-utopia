using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class ResourceExtractor : Producer
{
    public ResourceExtractor(Field, int);
    public override IndustrialResource Produce();
    public override Environmental Require();
};

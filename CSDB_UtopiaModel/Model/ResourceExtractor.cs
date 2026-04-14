using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
abstract public class ResourceExtractor : Producer
{
    public ResourceExtractor(Field field, int yield): base(field, yield) { }
    public override Environmental Require()  => Environmental.Instance(); 
};

using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class ResourceExtractor : Producer
{
    public ResourceExtractor(Field field, int x): base(field.X,field.Y) { }
    public override IndustrialResource Produce() { return IronOre.Instance(); }
    public override Enviromental Require() { return Enviromental.Instance(); }
};

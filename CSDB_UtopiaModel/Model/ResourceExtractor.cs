using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class ResourceExtractor : Producer
{
    public ResourceExtractor(Field field, int yield, Model model) : base(field, yield, model)
    {
        area = (2, 2);
    }

    public sealed override Environmental Require() => Environmental.Instance();
}
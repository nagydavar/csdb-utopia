using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class ResourceExtractor : Producer
{
    public ResourceExtractor(Field field, int yield, Model model) : base(field, yield, model)
    {
        area = (2, 2);
    }

    // Ha a Produce() és Require() típusa megegyezik, az is okozhat zavart.
    // A legjobb, ha egy bányának a Require() a saját termékét adja vissza, 
    // de a RequiredAmount = 0 miatt a Tick átugorja.
    public sealed override IResource Require() => Produce();
}
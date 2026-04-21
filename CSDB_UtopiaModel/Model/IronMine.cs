using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class IronMine : ResourceExtractor
{
    public override IronOre Produce() => IronOre.Instance();

    public override int RequiredAmount => 0;

    public override int ProducedAmount => 18;

    public IronMine(Field f, int yield, Model model) : base(f, yield, model)
    {
    }
}
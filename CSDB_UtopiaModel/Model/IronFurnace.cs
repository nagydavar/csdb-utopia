using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class IronFurnace : Factory
{
    public override Iron Produce() => Iron.Instance();
    public override IronOre Require() => IronOre.Instance();

    public override int RequiredAmount => 9;

    public override int ProducedAmount => 3;

    public IronFurnace(Field f, int yield, Model model) : base(f, yield, model)
    {
    }
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class SawMill : Factory
{
    public override Plank Produce() => Plank.Instance();
    public override Wood Require() => Wood.Instance();

    public override int RequiredAmount => 1;
    public override int ProducedAmount => 1;

    public override int placementCost => 1722;

    public SawMill(Field f, int yield, Model model) : base(f, yield, model)
    {
    }
}
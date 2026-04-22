using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class LumberYard : ResourceExtractor
{
    public override Wood Produce() => Wood.Instance();

    public override int RequiredAmount => 0;

    public override int ProducedAmount => 8;

    public override int placementCost => 1001;

    public LumberYard(Field f, int yield, Model model) : base(f, yield, model)
    {
    }
}
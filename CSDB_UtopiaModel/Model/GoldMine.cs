using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class GoldMine : ResourceExtractor
{
    public override Gold Produce() => Gold.Instance();

    public override int RequiredAmount => 0;

    public override int ProducedAmount => 3;

    public override int placementCost => 1500;

    public GoldMine(Field f, int yield, Model model) : base(f, yield, model)
    {
        area = (2, 2);
    }
}
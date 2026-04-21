using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class DiamondMine : ResourceExtractor
{
    public override Diamond Produce() => Diamond.Instance();

    public override int RequiredAmount => 0;

    public override int ProducedAmount => 1;

    public DiamondMine(Field f, int yield, Model model) : base(f, yield, model)
    {
    }
}
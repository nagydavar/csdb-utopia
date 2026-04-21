using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class CoalMine : ResourceExtractor
{
    public override Coal Produce() => Coal.Instance();

    public override int RequiredAmount => 0;

    public override int ProducedAmount => 20;

    public CoalMine(Field f, int yield, Model model) : base(f, yield, model)
    {
    }
}
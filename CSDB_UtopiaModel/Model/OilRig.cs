using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class OilRig : ResourceExtractor
{
    public override Oil Produce() => Oil.Instance();

    public override int RequiredAmount => 0;

    public override int ProducedAmount => 5;

    public override int placementCost => 666;

    public OilRig(Field f, int yield, Model model) : base(f, yield, model)
    {
    }
}
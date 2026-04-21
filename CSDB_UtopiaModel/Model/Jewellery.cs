using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class Jewellery : Factory
{
    public override Jewelry Produce() => Jewelry.Instance();
    public override Treasure Require() => Gold.Instance();

    public override int RequiredAmount => 3;
    
    public override int ProducedAmount => 1;

    public Jewellery(Field f, int yield, Model model) : base(f, yield, model)
    {
    }
}
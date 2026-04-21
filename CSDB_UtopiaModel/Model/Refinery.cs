using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class Refinery : Factory
{
    public Refinery(Field field, int yield, Model model) : base(field, yield, model)
    {
    }

    public override Gasoline Produce() => Gasoline.Instance();
    public override Oil Require() => Oil.Instance();

    public override int RequiredAmount => 5;
    public override int ProducedAmount => 2;
}
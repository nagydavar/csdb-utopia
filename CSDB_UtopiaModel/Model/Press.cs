using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class Press : Factory
{
    public Press(Field f, int yield, Model model) : base(f, yield, model)
    {
    }

    public override Book Produce() => Book.Instance();
    public override Paper Require() => Paper.Instance();

    public override int RequiredAmount => 10;
    public override int ProducedAmount => 1;
}
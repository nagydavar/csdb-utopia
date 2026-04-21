using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class PaperFactory : Factory
{
    public override Paper Produce() => Paper.Instance();
    public override Wood Require() => Wood.Instance();

    public override int RequiredAmount => 4;
    
    public override  int ProducedAmount => 2;

    public PaperFactory(Field f, int yield, Model model) : base(f, yield, model)
    {
    }
}
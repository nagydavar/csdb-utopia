using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Press : Factory
{
    public Press(Field f, int yield): base(f, yield) { }
    public override Book Produce() => Book.Instance();
    public override Paper Require() => Paper.Instance();
};

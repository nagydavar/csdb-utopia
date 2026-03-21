using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Refinery : Factory
{
    public Refinery(Field field, int yield) : base(field, yield) { }
    public override Gasoline Produce() => Gasoline.Instance();
    public override Oil Require() => Oil.Instance();
};

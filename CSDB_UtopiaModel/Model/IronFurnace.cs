using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class IronFurnace : Factory
{
        public override Iron Produce() => Iron.Instance();
        public override IronOre Require() => IronOre.Instance();
        public IronFurnace(Field f, int yield): base(f, yield) {}
    };

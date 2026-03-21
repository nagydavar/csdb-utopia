using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class IronMine : ResourceExtractor
{
        public override IronOre Produce() => IronOre.Instance();
        public IronMine(Field f, int yield): base(f,  yield) {}
    };

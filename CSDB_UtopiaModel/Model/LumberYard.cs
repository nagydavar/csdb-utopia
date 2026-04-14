using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class LumberYard : ResourceExtractor
{
        public override Wood Produce() => Wood.Instance();
        public LumberYard(Field f, int yield) :base(f, yield) {}
    };

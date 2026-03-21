using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class CoalMine : ResourceExtractor
{
        public override Coal Produce() => Coal.Instance();
        public CoalMine(Field f, int yield): base(f, yield) {}
    };

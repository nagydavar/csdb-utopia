using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class OilRig : ResourceExtractor
{
        public override Oil Produce() => Oil.Instance();
        public OilRig(Field f, int yield): base(f, yield) {}
    };

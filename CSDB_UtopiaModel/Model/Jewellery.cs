using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Jewellery: Factory
{
        public override Jewelry Produce() => Jewelry.Instance();
        public override Treasure Require() => Gold.Instance();
        public Jewellery(Field f, int yield): base(f, yield) {}
    };

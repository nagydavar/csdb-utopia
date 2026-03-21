using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class GoldMine : ResourceExtractor
{
        public override Gold Produce() => Gold.Instance();
        public GoldMine(Field f, int yield): base(f, yield) {}
};

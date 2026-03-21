using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class DiamondMine : ResourceExtractor
{
        public override Diamond Produce() => Diamond.Instance();
        public DiamondMine(Field f, int yield) : base(f, yield) {}
};

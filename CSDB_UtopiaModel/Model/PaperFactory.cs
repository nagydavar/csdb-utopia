using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class PaperFactory : Factory
{
        public override Paper Produce() => Paper.Instance();
        public override Wood Require() => Wood.Instance();
        public PaperFactory(Field f, int yield) :base(f, yield) {}
    };

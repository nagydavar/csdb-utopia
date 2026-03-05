using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class IronFurnace : Factory
{
        public Iron Produce();
        public Ironore Require();
    };

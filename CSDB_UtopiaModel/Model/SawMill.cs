using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class SawMill : Factory
{
        public override Plank Produce() => Plank.Instance();
        public override Wood Require() => Wood.Instance();
        public SawMill(Field f, int yield): base(f, yield) {}
    };

using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Factory : Producer
{

        public void Factory(Field, int);

        public override Goods Produce();
        public override Resource Require();
    };

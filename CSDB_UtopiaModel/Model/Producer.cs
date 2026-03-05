using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Producer : Building, Tickable
{
        protected int capacity;
        protected bool finished;
        protected bool isEmpty;
        protected bool gotResource;
        public Producer(Field, int);
        public Resource Produce();
        public Resource Require();
        public Tick();
    };

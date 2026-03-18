using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public abstract class Producer : Building, Tickable
{
        protected int capacity;
        protected bool finished;
        protected bool isEmpty;
        protected bool gotResource;
    public Producer(Field field, int x): base(field.X,field.Y) { }

    public Producer(int x, int y) : base(x, y)
    {
    }

    public abstract Resource Produce();
    public abstract Resource Require();
    public virtual void Tick() { }
};

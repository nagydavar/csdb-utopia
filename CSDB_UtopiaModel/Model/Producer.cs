using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public abstract class Producer : Building, Tickable
{
        protected int capacity;
        protected int yield;
        protected bool finished;
        protected bool isEmpty;
        protected bool gotResource;
    
    public Producer(Field f, int yield) : base(f)
    {
        this.yield = yield;
    }

    public abstract IResource Produce();
    public abstract IResource Require();
    public virtual void Tick() { }
};

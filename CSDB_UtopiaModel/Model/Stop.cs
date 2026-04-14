using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Stop: Buildable, Navigable
{
        public List<Building> connectsTo;
        public List<IResource> accept;
        public Dictionary<IResource, int> Resource;
        //Valahogy a Resourcenak egy megszorításnak kellene lennie, jó esetben statikus;

        public Stop(Field f): base(f) {}
        public void Load(IResource r, int amount) => throw new NotImplementedException();
        public int Unload(IResource r, int amount) => throw new NotImplementedException();
        public void AddBuildingsInRange() => throw new NotImplementedException();
        public void MoveTo() => throw new NotImplementedException();
    };

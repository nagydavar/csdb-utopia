using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Stop: Buildable, Navigable
{
        public List<Building> connectsTo;
        public List<Resource> accept;
        public Dictionary<Resource, int> Resource;
        //Valahogy a Resourcenak egy megszorításnak kellene lennie, jó esetben statikus;

        public Stop(Field f): base(f) {}
        public void Load(Resource r, int amount) => throw new NotImplementedException();
        public int Unload(Resource r, int amount) => throw new NotImplementedException();
        public void AddBuildingsInRange() => throw new NotImplementedException();
        public void MoveTo() => throw new NotImplementedException();
    };

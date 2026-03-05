using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Stop: Buildable, Navigable
{
        public List<Building> connectsTo;
        public List<Resource> accept;
        public Dictionary<Resource, int> Resource;
        %%Valahogy a Resourcenak egy megszorításnak kellene lennie, jó esetben statikus;

        public void Stop(int,int);
        public void Load(Resource, int);
        public int Unload(Resource, int);
        public void AddBuildingsInRange();
        public override MoveTo();
    };

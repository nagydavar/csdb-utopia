using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Stop: Buildable, INavigable
{
        public List<Building> connectsTo = new List<Building>();
        public HashSet<IVehicle> vehicles = new HashSet<IVehicle>();
        public List<IResource> accept = new List<IResource>();
        public Dictionary<IResource, int> Resource;
        //Valahogy a Resourcenak egy megszorításnak kellene lennie, jó esetben statikus;
        public override int placementCost => 200;

        public Stop(Field f) : base(f)
        {
            area = (1, 1);
        }

        public void MoveTo(IDirection dir, IVehicle vehicle) => vehicles.Add(vehicle);
        public void Leave(IVehicle vehicle) => vehicles.Remove(vehicle);

        public void Load(IResource r, int amount) => throw new NotImplementedException();
        public int Unload(IResource r, int amount) => throw new NotImplementedException();
        public void AddBuildingsInRange() => throw new NotImplementedException();
    
    };

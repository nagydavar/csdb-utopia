using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Garage : Building
{
        private List<Vehicle<Resource>>? vehiclesInGarage;

        public Garage(Field f): base(f) {}

        public void RepairCar(Vehicle<Resource> v)
        {
            throw new NotImplementedException();
        }
    };

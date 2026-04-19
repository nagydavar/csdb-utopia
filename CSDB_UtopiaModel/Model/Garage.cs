using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Garage : Building
{
    private List<Vehicle<IResource>>? vehiclesInGarage;
        
    public Garage(Field f): base(f) {
        area = (1, 1);
    }

    public override int placementCost => 800;

    public void RepairCar(Vehicle<IResource> v)
        {
            throw new NotImplementedException();
        }
    };

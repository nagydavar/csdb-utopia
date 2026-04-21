using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Garage : INavigable
{
    private List<IVehicle> vehiclesInGarage = new List<IVehicle>();
    public override bool TryMoveTo(IDirection dir, IVehicle vehicle)
    {
        vehiclesInGarage.Add(vehicle);
        return true;
    }

    public override void Leave(IVehicle vehicle)
    {
        vehiclesInGarage.Remove(vehicle);
    }

    public Garage(Field f): base(f) {
        area = (1, 1);
    }

    public override int placementCost => 800;

    public void RepairCar(Vehicle<IResource> v)
        {
            throw new NotImplementedException();
        }
    };

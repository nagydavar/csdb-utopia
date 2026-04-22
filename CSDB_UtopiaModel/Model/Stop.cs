using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Stop : INavigable
{
    private List<Buildable> connectsTo = new List<Buildable>();
    private HashSet<IVehicle> vehicles = new HashSet<IVehicle>();
    public HashSet<IResource> Accept = new HashSet<IResource>();

    private Dictionary<IResource, int> storage;

    //Valahogy a Resourcenak egy megszorításnak kellene lennie, jó esetben statikus;
    public override int placementCost => 200;

    public Stop(Field f, IEnumerable<IResource> resources) : base(f)
    {
        Accept = new HashSet<IResource>(resources);
    }

    public Stop(Field f) : base(f)
    {
        area = (1, 1);
    }

    public void AddNewConnection(Buildable b)
    {
        if (b is IResidentialBuilding rb)
        {
            Accept.Add(HumanResource.Instance());
            connectsTo.Add(b);
            rb.ConnectsTo = this;
        }

        if (b is Producer p)
        {
            connectsTo.Add(p);
            Accept.Add(HumanResource.Instance());
            Accept.Add(p.Produce());
            p.ConnectsTo = this;
            
            if (p.Require() is not Environmental)
                Accept.Add(p.Require());
        }

    }

    public void RemoveConnection(Buildable b)
    {
        connectsTo.Remove(b);
        if (b is IResidentialBuilding rb)
            rb.ConnectsTo = null;

        if (b is Producer p)
            p.ConnectsTo = null;

    }

    public override bool TryMoveTo(IDirection dir, IVehicle vehicle)
    {
        vehicles.Add(vehicle);
        return true;
    }

    public override void Leave(IVehicle vehicle) => vehicles.Remove(vehicle);

    public bool IsVehicleOk(IVehicle vehicle) => vehicles.Contains(vehicle);

    public void UnLoad<Carriable>(Vehicle<Carriable> vehicle, Carriable resource, int pcs) where Carriable : IResource
    {
        //lepakol a jarmu
        if (!IsVehicleOk(vehicle)) throw new InvalidOperationException("Vehicle is not is the stop to load");
        if (!storage.TryAdd(resource, pcs))
            storage[resource] += pcs;
    }

    public (int, CarriedResource?) Load<CarriedResource>(Vehicle<CarriedResource> vehicle,
        HashSet<CarriedResource> carriable, int maxPcs) where CarriedResource : IResource
    {
        //felpakol a jarmu
        if (!IsVehicleOk(vehicle)) throw new InvalidOperationException("Vehicle is not is the stop to unload");


        List<CarriedResource> avaiable = (List<CarriedResource>)Accept.Where(r => r is CarriedResource)
            .Cast<CarriedResource>().Intersect(carriable).ToList();
        if (avaiable.Count == 0) return (0, default);
        int iRandom = RandomSingleton.Instance.Next(avaiable.Count);
        CarriedResource toLoad = (CarriedResource)avaiable.ToList()[iRandom];


        int maxPcsToTake = Math.Min(maxPcs, storage[toLoad]);
        return (maxPcsToTake, toLoad);
    }
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Stop : INavigable
{
    private readonly HashSet<IVehicle> _vehicles = new HashSet<IVehicle>();
    private readonly Dictionary<IResource, int> _storage = new();

    public override int placementCost => 200;

    public int this[IResource resource] => resource switch
    {
        Environmental => int.MaxValue,
        _ => _storage[resource]
    };

    public Stop(Field f) : base(f)
    {
        area = (1, 1);
    }

    public override bool TryMoveTo(IDirection dir, IVehicle vehicle)
    {
        _vehicles.Add(vehicle);
        return true;
    }

    public override void Leave(IVehicle vehicle) => _vehicles.Remove(vehicle);

    public void Load(IResource resource, int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        if (resource is Environmental) return;

        if (_storage.TryGetValue(resource, out int value))
            _storage[resource] = amount + value;
        else
            _storage.Add(resource, amount);
    }

    public int Unload(IResource resource, int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        if (resource is Environmental) return amount;

        if (!_storage.TryGetValue(resource, out int value))
            throw new ArgumentException("The stop does not contain this type of resource.", nameof(resource));

        if (value >= amount)
        {
            _storage.Remove(resource);
            return value;
        }

        // else
        _storage[resource] -= amount;
        return amount;
    }
}
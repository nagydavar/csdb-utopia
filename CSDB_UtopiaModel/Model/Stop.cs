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

    public int Unload(IResource resource, int requestedAmount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(requestedAmount);

        if (resource is Environmental) return requestedAmount;

        if (!_storage.TryGetValue(resource, out int availableAmount))
            return 0; // Ne dobj hibát, ha nincs ott semmi, csak mondd meg, hogy 0-t tudtunk adni

        // Kiszámoljuk, mennyit tudunk ténylegesen átadni
        int actualGiven = Math.Min(availableAmount, requestedAmount);

        _storage[resource] -= actualGiven;

        return actualGiven;
    }

    public (IResource? resource, int amount) UnloadAnythingValid(IVehicle vehicle, int requestAmount)
    {
        // Végigpörgetjük a megálló raktárát
        foreach (var entry in _storage)
        {
            IResource availableResource = entry.Key;

            // Megkérdezzük az autót: "Te el tudod vinni ezt?"
            if (vehicle.CanCarry(availableResource))
            {
                // Ha igen, az Unload metódusoddal kivesszük a raktárból
                int taken = this.Unload(availableResource, requestAmount);
                return (availableResource, taken);
            }
        }
        return (null, 0);
    }
}
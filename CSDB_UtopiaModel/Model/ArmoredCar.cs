using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class TreasureArmoredCar<TTreasure> : GoodsVehicle<TTreasure> where TTreasure : Persistence.Treasure
{
    public override int placementCost { get; } = 300;

    public TreasureArmoredCar(Map map, Model m) : base(map, m)
    {
        maintenanceCost = 100;
        speed = 100;
        capacity = 40;
    }
    public override bool CanCarry(IResource resource)
    {
        // Csak ha Gold VAGY Diamond
        return resource is Gold || resource is Diamond;
    }
}

public class ArmoredCar : TreasureArmoredCar<Treasure> { public ArmoredCar(Map m, Model mo) : base(m, mo) { } }
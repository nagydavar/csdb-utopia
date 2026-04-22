using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class TreasureArmoredCar<TTreasure> : GoodsVehicle<TTreasure> where TTreasure : Persistence.Treasure
{
    public override int placementCost { get; } = 300;
    
    public TreasureArmoredCar(Map map, Model m) : base(map,m)
    {
        maintenanceCost = 100;
        speed = 100;
        capacity = 40;
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}

public class ArmoredCar : TreasureArmoredCar<Treasure> { public ArmoredCar(Map m, Model mo) : base(m, mo) { } }
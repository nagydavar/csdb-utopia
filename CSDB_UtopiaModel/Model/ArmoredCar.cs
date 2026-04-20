namespace CSDB_UtopiaModel.Model;

class ArmoredCar<TTreasure> : GoodsVehicle<TTreasure> where TTreasure : Persistence.Treasure
{
    public ArmoredCar(Map map, Model m) : base(map,m)
    {
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
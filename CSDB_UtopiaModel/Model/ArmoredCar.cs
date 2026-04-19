namespace CSDB_UtopiaModel.Model;

class ArmoredCar<TTreasure> : GoodsVehicle<TTreasure> where TTreasure : Persistence.Treasure
{
    public ArmoredCar(Map map, Model m, Coordinate start, Coordinate end) : base(map,m, start, end)
    {
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
namespace CSDB_UtopiaModel.Model;

class Taxi : PassengerVehicle
{

    public override int placementCost { get; } = 150;
    public Taxi(Map map, Model m) : base(map,m)
    {
        maintenanceCost = 30;
        speed = 120;
        capacity = 3;
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
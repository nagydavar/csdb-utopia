namespace CSDB_UtopiaModel.Model;

class Car : PassengerVehicle
{
    public override int placementCost { get; } = 100;
    
    public Car(Map map, Model m) : base(map,m)
    {
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
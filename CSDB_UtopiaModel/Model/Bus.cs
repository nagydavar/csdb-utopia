namespace CSDB_UtopiaModel.Model;

public class Bus : PassengerVehicle
{
    public override int placementCost { get; } = 300;
    
    public Bus(Map map, Model m) :base(map, m)
    {
        speed = 90;
        maintenanceCost = 60;
        capacity = 50;
    }

    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
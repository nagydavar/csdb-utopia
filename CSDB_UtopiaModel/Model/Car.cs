namespace CSDB_UtopiaModel.Model;

class Car : PassengerVehicle
{
    public Car(Map map, Model m, Coordinate start, Coordinate end) : base(map,m, start, end)
    {
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
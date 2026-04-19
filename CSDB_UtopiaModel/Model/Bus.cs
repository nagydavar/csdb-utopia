namespace CSDB_UtopiaModel.Model;

class Bus : PassengerVehicle
{
    public Bus(Map map, Model m, Coordinate start, Coordinate end) : base(map,m, start, end)
    {
    }
    // private override int capacity;
    // private override int maintenanceCost;
    // private override int speed;
}
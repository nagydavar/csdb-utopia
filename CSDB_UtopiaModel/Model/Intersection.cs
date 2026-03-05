namespace CSDB_UtopiaModel.Model;
class Intersection {
        public bool hasTrafficLights;
        public TrafficLightControl trafficLightControl;
        public List<Vehicle> stuckedVehicles;
        public List<Pair<GoingIntention, Vehicle>> arrived;
        public void Intersection(Coords);
        public void Intersection(Coords, TrafficLightControl);
        public void NewGreen();
        public void NotifyNextVehicle();
        public void AddTrafficControl(TrafficLightControl);
        public TrafficLight? TrafficLight(Direction);
        public void ArriveWithIntention(Vehicle, GoingIntention);
        public void Tick();
    };

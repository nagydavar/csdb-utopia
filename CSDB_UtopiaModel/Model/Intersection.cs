using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Intersection {
        public bool hasTrafficLights;
        public TrafficLightControl trafficLightControl;
        public List<Vehicle<Resource>> stuckedVehicles;
        public List<(GoingIntention, Vehicle<Resource>)> arrived;
        public Intersection(Coordinate c) => throw new NotImplementedException();
        public Intersection(Coordinate c, TrafficLightControl tlc) => throw new NotImplementedException();
        public void NewGreen() => throw new NotImplementedException();
        public void NotifyNextVehicle() => throw new NotImplementedException();
        public void AddTrafficControl(TrafficLightControl tlc) => throw new NotImplementedException();
        public TrafficLight? TrafficLight(Direction d) => throw new NotImplementedException();
        public void ArriveWithIntention(Vehicle<Resource> v, GoingIntention gi) => throw new NotImplementedException();
        public void Tick() => throw new NotImplementedException();
    };

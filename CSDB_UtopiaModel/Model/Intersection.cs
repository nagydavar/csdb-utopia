using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public abstract class Intersection {
        public bool hasTrafficLights;
        public TrafficLightControl trafficLightControl;
        public List<Vehicle<IResource>> stuckedVehicles;
        public List<(GoingIntention, Vehicle<IResource>)> arrived;
        public IDirection TrafficLightIDirection { get; private set; }
        public Intersection(Field f, IDirection dir) { TrafficLightIDirection = dir; }
        public Intersection(Field f, TrafficLightControl tlc) => throw new NotImplementedException();
        public void NewGreen() => throw new NotImplementedException();
        public void NotifyNextVehicle() => throw new NotImplementedException();
        public void AddTrafficControl(TrafficLightControl tlc) => throw new NotImplementedException();
        public TrafficLight? TrafficLight(IDirection d) => throw new NotImplementedException();
        public void ArriveWithIntention(Vehicle<IResource> v, GoingIntention gi) => throw new NotImplementedException();
        public void Tick() => throw new NotImplementedException();
    };

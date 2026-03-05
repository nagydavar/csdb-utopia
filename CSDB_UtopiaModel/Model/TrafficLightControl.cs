namespace CSDB_UtopiaModel.Model;
class TrafficLightControl: Tickable
{
        public Dictionary<Direction, TrafficLight> TrafficLights;
        public Intersecion intersection;
        public TrafficLightSchedule schedule;
        public TrafficLight TrafficLight(Direction);
        public void TrafficLightControl(Intersection, TrafficLightSchedule, Dictionary<Direction, TrafficLight>);
        public void Tick();

    };

using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class TrafficLightControl : ITickable
{
    public Dictionary<Direction, TrafficLight> TrafficLights { get; set; } // !!!
    public Intersection Intersection { get; set; }
    public TrafficLightSchedule Schedule { get; set; }

    public TrafficLightControl(Intersection _, TrafficLightSchedule b, Dictionary<Direction, TrafficLight> a) =>
        throw new NotImplementedException();

    public TrafficLight TrafficLight(Direction _) => throw new NotImplementedException();
    public Task Tick() => throw new NotImplementedException();
}
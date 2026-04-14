using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class TrafficLightControl : ITickable
{
    public Dictionary<IDirection, TrafficLight> TrafficLights { get; set; } // !!!
    public Intersection Intersection { get; set; }
    public TrafficLightSchedule Schedule { get; set; }

    public TrafficLightControl(Intersection _, TrafficLightSchedule b, Dictionary<IDirection, TrafficLight> a) =>
        throw new NotImplementedException();

    public TrafficLight TrafficLight(IDirection _) => throw new NotImplementedException();
    public Task Tick() => throw new NotImplementedException();
}
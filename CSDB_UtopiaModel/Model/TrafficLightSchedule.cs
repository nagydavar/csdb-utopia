namespace CSDB_UtopiaModel.Model;

public abstract class TrafficLightSchedule
{
    public List<(TrafficLightSection, int)> Desc { get; protected set; }
}
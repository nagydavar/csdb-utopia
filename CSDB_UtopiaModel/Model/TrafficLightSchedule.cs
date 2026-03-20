namespace CSDB_UtopiaModel.Model;

abstract class TrafficLightSchedule
{
    public List<(TrafficLightSection, int)> Desc { get; protected set; }
}
namespace CSDB_UtopiaModel.Model;

class TrafficLight
{
    public TrafficLightState State { get; set; } // do we need this?
    public bool IsGreen(GoingIntention _) => throw new NotImplementedException();

    public EventHandler<GoingIntentionEventArgs> TurnedGreen;

    public void Turn(TrafficLightState _, GoingIntention _) => throw new NotImplementedException();
}
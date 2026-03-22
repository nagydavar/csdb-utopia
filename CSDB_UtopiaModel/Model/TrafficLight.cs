namespace CSDB_UtopiaModel.Model;

class TrafficLight
{
    public TrafficLightState State { get; set; } // do we need this?

    public EventHandler<GoingIntentionEventArgs> TurnedGreen;

    public bool IsGreen(GoingIntention _) => throw new NotImplementedException();
    public void Turn(TrafficLightState _, GoingIntention a) => throw new NotImplementedException();
}
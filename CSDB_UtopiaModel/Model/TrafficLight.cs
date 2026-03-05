namespace CSDB_UtopiaModel.Model;
class TrafficLight {
        public TrafficLightState state;
        public void IsGreen(GoingIntention):bool;
        public EventHandler<GoingIntetionEventArgs> TurnedGreen;
        public void Turn(TrafficLightState, GoingIntention);

    };

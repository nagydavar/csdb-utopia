using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Vehicle<Resource> {
        protected int capacity;
        protected int maintenanceCost;
        protected int speed;
        protected Navigator navi;
        protected LinkedList<Field>? PathToGarage;

        public Field position;
        public int TimeSinceBought;
        public int TimeSpentOnCurrentRoad;
        public int ThresholdToMove;

        public void Vehicle(Field, int, int, int, Navigator);
        protected TimeToGoToGarage() bool;
        protected CalculatePathToNearestGarage() LinkedList<Field>;
        public void GoToGarage();
        public void GoBackToPath();
        public int Sell();
        public override Tick();
        public int GetPositionInField();
        public GoingIntention GetIntention();

    };

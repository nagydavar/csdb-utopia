using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Model {

        private TimeControl TimeControl;
        private Persistence persistence;
        public Model();
        public void Place(int, int, Buildable);
        public  PlaceVehicle(int, int, Vehicle);


        public  AddVehicle(Vehicle);
        public  Demolish(int, int);

        public void ListBuildableFactories();
        public void ListBuildableProducers();
        public void ListBuildableDecorations();
        public void ListBuildableOtherBuildings();
        public void ListBuildableRoads();
        public void ListBuyablePassengerVehicles();
        public void ListBuyableIndustrialVehicles();
;
        public EventHandler<EventArgs>? GameTicked;
        public EventHandler<FieldEventArgs>? FieldsUpdated;
        public EventHandler<EventArgs>? BudgetChanged;
        public EventHandler<ResourceChangedEventArgs>? ResourceChanged;
        public EventHandler<LogEventArgs>? NewLog;
        public EventHandler<EventArgs>? NewGame;
        public EventHandler<EventArgs>? GameOver;
        public EventHandler<EventArgs>? DateChanged;

    };

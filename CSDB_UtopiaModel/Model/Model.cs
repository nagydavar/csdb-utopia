using CSDB_UtopiaModel.Persistence;
using System;

namespace CSDB_UtopiaModel.Model;
class Model {

    private TimeControl _timeControl;
    private readonly Persistence.Persistence _persistence;
    public Model(int width, int height) {
        _persistence = new Persistence.Persistence(width, height);
    }
    public void Place(int x, int y, Buildable buildable) {
        if (buildable is IResidentialBuilding residential)
        {
            // Népesség növelése
            _persistence.Storage[HumanResource.Instance()] += residential.givePeople;
            OnResourceChanged(HumanResource.Instance(), _persistence.Storage[HumanResource.Instance()]);

            // Hangulat csökkentése
            _persistence.CurrentMood += residential.AffectMood;
            OnMoodChanged(_persistence.CurrentMood);

            // Itt jönne a mezõ frissítése a térképen most csak teszt miatt, de így is gatya
            Console.WriteLine($"Építve: {x},{y} koordinátán. Pop: {_persistence.Storage[HumanResource.Instance()]}, Mood: {_persistence.CurrentMood}");
        } 
    }
    public void PlaceVehicle(int x , int y , Vehicle<Resource> vehicle) { }


    public void AddVehicle(Vehicle<Resource> vehicle) { }
    public void Demolish(int x, int y) { }

    public void ListBuildableFactories() { }
    public void ListBuildableProducers() { }
    public void ListBuildableDecorations() { }
    public void ListBuildableOtherBuildings() { }
    public void ListBuildableRoads() { }
    public void ListBuyablePassengerVehicles() { }
    public void ListBuyableIndustrialVehicles() { }

    public EventHandler<EventArgs>? GameTicked;
    public EventHandler<FieldEventArgs>? FieldsUpdated;
    public EventHandler<EventArgs>? BudgetChanged;
    public EventHandler<ResourceChangedEventArgs>? ResourceChanged;
    public EventHandler<MoodChangedEventArgs>? MoodChanged;
    public EventHandler<LogEventArgs>? NewLog;
    public EventHandler<EventArgs>? NewGame;
    public EventHandler<EventArgs>? GameOver;
    public EventHandler<EventArgs>? DateChanged;

    protected virtual void OnResourceChanged(Resource resource, int newValue)
    {
        ResourceChanged?.Invoke(this, new ResourceChangedEventArgs(resource, newValue));
    }
    protected virtual void OnMoodChanged( int newValue)
    {
        MoodChanged?.Invoke(this, new MoodChangedEventArgs(newValue));
    }

};

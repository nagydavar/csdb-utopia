using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Model
{

    private TimeControl _timeControl;
    private readonly Persistence.Persistence _persistence;

    public Model(int width, int height)
    {
        _persistence = new Persistence.Persistence(width, height, true);
    }

    public void Place(Coordinate coord, Buildable buildable)
    {
        if (buildable is IResidentialBuilding residential)
        {
            // N�pess�g n�vel�se
            _persistence.Storage[HumanResource.Instance()] += residential.givePeople;
            OnResourceChanged(HumanResource.Instance(), _persistence.Storage[HumanResource.Instance()]);

            // Hangulat cs�kkent�se
            _persistence.CurrentMood += residential.AffectMood;
            OnMoodChanged(_persistence.CurrentMood);

            // Itt j�nne a mez� friss�t�se a t�rk�pen most csak teszt miatt, de �gy is gatya
            // Console.WriteLine(
            //     $"�p�tve: {x},{y} koordin�t�n. Pop: {_persistence.Storage[HumanResource.Instance()]}, Mood: {_persistence.CurrentMood}");
        }
        _persistence.Fields[coord.X][coord.Y].Place(buildable);
        _persistence.Budget -= buildable.placementCost;
    }

    public void PlaceVehicle(Coordinate coord, Vehicle<Resource> vehicle)
    {
        if (_persistence.Fields[coord.X][coord.Y].Buildable is not Road road)
            throw new InvalidOperationException("You can only place a vehicle to a road");

        throw new NotImplementedException();
    }

    //nyersanyag friss�t�se miatt
    public int GetBudget() // should be a property
    {
        return _persistence.Budget;
    }

    public int GetMood() // should be a property
    {
        return _persistence.CurrentMood;
    }

    public int GetResourceCount(Resource resource)
    {
        return _persistence.Storage.ContainsKey(resource) ? _persistence.Storage[resource] : 0;
    }
    // id�ig �j

    public void AddVehicle(Vehicle<Resource> vehicle)
    {
    }

    public void Demolish(Coordinate coord)
    {
        // if (/*undemolishable*/)
        //     throw new Exception("ejnye-bejnye!");
        _persistence.Fields[coord.X][coord.Y].Buildable = null;
    }

    public void ListBuildableFactories()
    {
    }

    public void ListBuildableProducers()
    {
    }

    public void ListBuildableDecorations()
    {
    }

    public void ListBuildableOtherBuildings()
    {
    }

    public void ListBuildableRoads()
    {
    }

    public void ListBuyablePassengerVehicles()
    {
    }

    public void ListBuyableIndustrialVehicles()
    {
    }

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

    protected virtual void OnMoodChanged(int newValue)
    {
        MoodChanged?.Invoke(this, new MoodChangedEventArgs(newValue));
    }
}
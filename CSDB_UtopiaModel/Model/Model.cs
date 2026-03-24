using System.Reflection;
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
        Field target = _persistence.Fields[coord.X][coord.Y];
        if (target is not Land land) return;
        if (buildable.placementCost > GetBudget()) return;
        
        land.Place(buildable);
        _persistence.Budget -= buildable.placementCost;

        BudgetChanged?.Invoke(this, EventArgs.Empty);

        OnFieldsUpdated(_persistence.Fields[coord.X][coord.Y]);
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

    public Field GetField(int x, int y) {
        return _persistence.Fields[x][y];
    }
    public Field GetField(Coordinate coords) => GetField(coords.X, coords.Y);
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
        Buildable? onField = GetField(coord).Buildable;
        
        _persistence.Fields[coord.X][coord.Y].Buildable = null;

        OnFieldsUpdated(_persistence.Fields[coord.X][coord.Y]);

        if (onField is IResidentialBuilding residential)
        {
            _persistence.Storage[HumanResource.Instance()] -= residential.givePeople;
            OnResourceChanged(HumanResource.Instance(), _persistence.Storage[HumanResource.Instance()]);
        }
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

    // public Buildable[] ListBuildableOtherBuildings() =>
    // [
    //     new LumberYard(default!, default), new IronMine(default!, default), new CoalMine(default!, default),
    //     new OilRig(default!, default), new GoldMine(default!, default), new DiamondMine(default!, default),
    //     new IronFurnace(default!, default), new SawMill(default!, default), new Refinery(default!, default),
    //     new Jewellery(default!, default), new PaperFactory(default!, default), new Press(default!, default),
    //     // to be continued
    //     new Stop(null!)
    // ];

    public List<Type> ListBuildableOtherBuildings()
    {
        string targetNamespace = typeof(CSDB_UtopiaModel.Model.Model).Namespace;

        List<Type> types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(CSDB_UtopiaModel.Model.IResidentialBuilding)) && t.Namespace == targetNamespace)
            .ToList();
        
        return types;
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

    protected virtual void OnFieldsUpdated(Field field)
    {
        FieldsUpdated?.Invoke(this, new FieldEventArgs(new List<Field> { field }));
    }
}
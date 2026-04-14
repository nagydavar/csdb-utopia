using System.Reflection;
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Model : ITickable
{

    private TimeControl _timeControl;
    private readonly Persistence.Persistence _persistence;
    private int _totalSeconds = 0; // Az eltelt összes másodperc

    public Model(int width, int height)
    {
        _persistence = new Persistence.Persistence(width, height, true);

        // 1. Lekérjük a példányt egy változóba
        var timer = TimeControl.Instance();

        // 2. A változón már elvégezhető az operátor-művelet
        timer += (this, 1);
    }

    #region Time
    public Task Tick()
    {
        _totalSeconds++;

        // Kiváltjuk az eseményt a View felé
        DateChanged?.Invoke(this, EventArgs.Empty);

        return Task.CompletedTask;
    }

    // Formázott idő lekérése a ViewModel számára
    public string GetFormattedTime()
    {
        int hours = _totalSeconds / 3600;
        int minutes = (_totalSeconds % 3600) / 60;
        int seconds = _totalSeconds % 60;
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    // Vezérlő metódusok a TimeControl-hoz
    public void TogglePause()
    {
        var timer = TimeControl.Instance();
        _ = !timer; // A '!' operátor meghívása
    }
    public void SpeedUp()
    {
        var timer = TimeControl.Instance();
        _ = ++timer;
    }
    public void SlowDown()
    {
        var timer = TimeControl.Instance();
        _ = --timer;
    }

    public bool IsPaused() => !TimeControl.Instance().IsStopped;
    #endregion

    public void Place(Coordinate coord, Buildable buildable)
    { 
       
        // 1. Épület méretének lekérése
        int width = buildable.area.Item1;
        int height = buildable.area.Item2;
        List<Land> targetLands = new List<Land>();
        float totalForestFactor = 0;
        (Resource?, int) resource = (null,0);

        // 2. Ellenőrzés, elfér-e és minden mező Land-e?
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Koordináta validáció (ne lógjon ki a pályáról)
                if (coord.X + i >= _persistence.Width || coord.Y + j >= _persistence.Height) return;

                Field f = _persistence.Fields[coord.X + i][coord.Y + j];

                // Csak szabad Land mezőre építhetünk
                if (f is Land land && !land.HasBuildable)
                {
                    targetLands.Add(land);
                    totalForestFactor += 0.05f * land.LevelOfForest;
                }
                else return; // Ha csak egyetlen mező is foglalt vagy nem Land, megállunk
            }
        }

        //3. költség
        int cost = (int)Math.Round((1 + totalForestFactor / (width * height)) * buildable.placementCost, 0);

        if (cost > GetBudget()) return;
        if (buildable is Decoration decor)
        {
            resource = decor.costResource;
            if (_persistence.Storage[decor.costResource.resource] < decor.costResource.cost) return;
        }

        //4. elhelyezés
        foreach (var land in targetLands)
        {
            // Kiszámoljuk a relatív pozíciót a kezdőponthoz képest
            land.RelativeX = land.Coordinates.X - coord.X;
            land.RelativeY = land.Coordinates.Y - coord.Y;

            land.Place(buildable); // Ez meghívja a Deforest()-et is az adott mezőn
        }

        //5. frissítés
        _persistence.Budget -= cost;
        BudgetChanged?.Invoke(this, EventArgs.Empty);

        if (buildable is Decoration decoration) {
            _persistence.Storage[resource.Item1!] -= resource.Item2;
            OnResourceChanged(resource.Item1!, _persistence.Storage[resource.Item1!]);

            _persistence.CurrentMood += decoration.giveMood;
            OnMoodChanged(_persistence.CurrentMood);
        }

        // Minden megváltozott mezőt elküldünk a View-nak
        foreach (var land in targetLands)
        {
            OnFieldsUpdated(land);
        }

        if (buildable is IResidentialBuilding residential)
        {
            // N�pess�g n�vel�se
            _persistence.Storage[HumanResource.Instance()] += residential.givePeople;
            OnResourceChanged(HumanResource.Instance(), _persistence.Storage[HumanResource.Instance()]);

            // Hangulat cs�kkent�se
            _persistence.CurrentMood += residential.AffectMood;
            OnMoodChanged(_persistence.CurrentMood);
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
        Field field = GetField(coord);

        Field source = GetField(new Coordinate(coord.X-field.RelativeX, coord.Y-field.RelativeY));
        if (onField is not null)
        {
            for (int i = 0; i < onField.area.Width; i++)
            {
                for (int j = 0; j < onField.area.Height; j++)
                {
                    Coordinate currentCoord = new Coordinate(source.Coordinates.X + i, source.Coordinates.Y + j);

                    // Biztonsági ellenőrzés, ne lógjunk ki a pályáról
                    if (currentCoord.X < _persistence.Width && currentCoord.Y < _persistence.Height)
                    {
                        Field f = GetField(currentCoord);
                        f.Buildable = null;
                        f.RelativeX = 0;
                        f.RelativeY = 0;
                        OnFieldsUpdated(f);
                    }
                }
            }

            if (onField is IResidentialBuilding residential)
            {
                //népesség csökkentése
                _persistence.Storage[HumanResource.Instance()] -= residential.givePeople;
                OnResourceChanged(HumanResource.Instance(), _persistence.Storage[HumanResource.Instance()]);

                //Hangulat visszaadása
                _persistence.CurrentMood += (-1) * residential.AffectMood; //mert negatív
                OnMoodChanged(_persistence.CurrentMood);
            }

            if (onField is Decoration decor)
            {
                //hangulat csökkentése
                _persistence.CurrentMood -= decor.giveMood;
                OnMoodChanged(_persistence.CurrentMood);
            }
        }
    }

    public List<Type> ListBuildableFactories()
    {
        return GetBuildableTypesInNamespace<Factory>();
    }

    public List<Type> ListBuildableResourceExtractors()
    {
        return GetBuildableTypesInNamespace<ResourceExtractor>();
    }

    public List<Type> ListBuildableDecorations()
    {
        return GetBuildableTypesInNamespace<Decoration>();
    }

    public List<Type> ListBuildableOtherBuildings()
    {
        string targetNamespace = typeof(CSDB_UtopiaModel.Model.Model).Namespace;

        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        t.IsAssignableTo(typeof(Buildable)) &&
                        // Kizárjuk a már nevesített kategóriákat
                        !t.IsAssignableTo(typeof(IResidentialBuilding)) &&
                        !t.IsAssignableTo(typeof(Decoration)) &&
                        !t.IsAssignableTo(typeof(Factory)) &&
                        !t.IsAssignableTo(typeof(ResourceExtractor)) &&
                        !t.IsAssignableTo(typeof(Road)) &&
                        t.Namespace == targetNamespace)
            .ToList();
    }

    public List<Type> ListBuildableResidential()
    {
        return GetBuildableTypesInNamespace<IResidentialBuilding>();
    }

    public List<Type> ListBuildableRoads()
    {
        return GetBuildableTypesInNamespace<Road>();
    }

    public List<Type> ListBuyablePassengerVehicles()
    {
        return GetBuildableTypesInNamespace<PassengerVehicle>();
    }

    public List<Type> ListBuyableIndustrialVehicles()
    {
        //TODO
        return GetBuildableTypesInNamespace<Decoration>();
    }

    private List<Type> GetBuildableTypesInNamespace<T>()
    {
        string targetNamespace = typeof(CSDB_UtopiaModel.Model.Model).Namespace;

        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        t.IsAssignableTo(typeof(T)) &&
                        t.Namespace == targetNamespace)
            .ToList();
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
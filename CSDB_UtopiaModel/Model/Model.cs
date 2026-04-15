using System.Reflection;
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Model : ITickable
{

    private TimeControl _timeControl;
    private Persistence.Persistence _persistence;
    private int _totalSeconds = 0; // Az eltelt összes másodperc

    public EventHandler<EventArgs>? GameTicked;
    public EventHandler<FieldEventArgs>? FieldsUpdated;
    public EventHandler<EventArgs>? BudgetChanged;
    public EventHandler<ResourceChangedEventArgs>? ResourceChanged;
    public EventHandler<MoodChangedEventArgs>? MoodChanged;
    public EventHandler<LogEventArgs>? NewLog;
    public EventHandler<EventArgs>? NewGame;
    public EventHandler<EventArgs>? GameOver;
    public EventHandler<EventArgs>? DateChanged;

    public Model(int width, int height)
    {
        _persistence = new Persistence.Persistence(width, height, true);

        RegisterEvents();

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

    public void Reset(int width, int height)
    {
        // Új perzisztencia réteg létrehozása
        _persistence = new Persistence.Persistence(width, height, true);

        RegisterEvents();

        _totalSeconds = 0;

        // Értesítjük a View-t, hogy minden adat (mezők, büdzsé) megváltozott
        NewGame?.Invoke(this, EventArgs.Empty);

        // Frissítjük az összes mezőt a UI-on
        for (int i = 0; i < width; i++)
        for (int j = 0; j < height; j++)
            OnFieldsUpdated(_persistence.Fields[i][j]);

        BudgetChanged?.Invoke(this, EventArgs.Empty);
        MoodChanged?.Invoke(this, new MoodChangedEventArgs(_persistence.CurrentMood));
    }

    public void PlaceRoad(Coordinate coord)
    {
        int tmp;
        Field?[] roadArray = [null, null, null, null];
        List<Field> roads = new(4);

        if ((tmp = coord.Y - 1) >= 0 && _persistence.Fields[coord.X][tmp].Buildable is Road)
        {
            roads.Add(roadArray[0] = _persistence.Fields[coord.X][tmp]);
        }

        if ((tmp = coord.X + 1) < _persistence.Fields.Count && _persistence.Fields[tmp][coord.Y].Buildable is Road)
        {
            roads.Add(roadArray[1] = _persistence.Fields[tmp][coord.Y]);
        }

        if ((tmp = coord.Y + 1) < _persistence.Fields[coord.X].Count &&
            _persistence.Fields[coord.X][tmp].Buildable is Road)
        {
            roads.Add(roadArray[2] = _persistence.Fields[coord.X][tmp]);
        }

        if ((tmp = coord.X - 1) >= 0 && _persistence.Fields[tmp][coord.Y].Buildable is Road)
        {
            roads.Add(roadArray[3] = _persistence.Fields[tmp][coord.Y]);
        }

        bool isCurved, becomeIntersection = roads.Count is 3 or 4;
        byte q = 0;
        IDirection dir = Up.Instance();
        Intersection? intersection=null;
        Field f = _persistence.Fields[coord.X][coord.Y];
        switch (roads.Count)
        {
            case 1:
                isCurved = false;
                q = 0;

                switch (Array.IndexOf(roadArray, roads[0]))
                {
                    case 0:
                        dir = Up.Instance();
                        break;
                    case 1:
                        dir = Right.Instance();
                        break;
                    case 2:
                        Down.Instance();
                        break;
                    case 3:
                        Left.Instance();
                        break;
                    default:
                        return; // error occured, road cannot be placed
                }

                break;
            case 2:
                if (roads[0].Coordinates.X == roads[1].Coordinates.X)
                {
                    isCurved = false;
                    dir = Right.Instance();
                    q = 0;
                }
                else if (roads[0].Coordinates.Y == roads[1].Coordinates.Y)
                {
                    isCurved = false;
                    dir = Up.Instance();
                    q = 0;
                }
                else if (coord.Y - roads[0].Coordinates.Y > 0 || coord.Y - roads[1].Coordinates.Y > 0) // Up
                {
                    isCurved = true;
                    dir = Up.Instance(); // this has no meaning

                    if (coord.X - roads[0].Coordinates.X > 0 || coord.X - roads[1].Coordinates.X > 0) // Left
                    {
                        q = 2;
                    }
                    else // Right
                    {
                        q = 1;
                    }
                }
                else // Down
                {
                    isCurved = true;
                    dir = Down.Instance(); // this has no meaning 

                    if (coord.X - roads[0].Coordinates.X > 0 || coord.X - roads[1].Coordinates.X > 0) // Left
                        q = 3;
                    else // Right
                        q = 4;
                }

                break;
            case 3:
                foreach (Field field in roads)
                {
                    if (field.Buildable is Motorway { HasIntersection: true })
                        return; // can't place two intersections next to each other
                }

                isCurved = false;
                q = 0;

                // assume that we put the elements in the list clockwise
                switch (Array.IndexOf(roadArray, null))
                {
                    case 0:
                        dir = Down.Instance();
                        break;
                    case 1:
                        dir = Left.Instance();
                        break;
                    case 2:
                        dir = Up.Instance();
                        break;
                    case 3:
                        dir = Right.Instance();
                        break;
                    default:
                        return; // error occured, road cannot be placed
                }

                intersection = new ThreeWayIntersection(f, dir);

                break;
            case 4:
                foreach (Field field in roads)
                {
                    if (field.Buildable is Motorway { HasIntersection: true })
                        return; // can't place two intersections next to each other
                }

                isCurved = false;
                // dummy values:
                q = 0;
                dir = Up.Instance();

                intersection = new FourWayIntersection(f);
                break;
            default:
                return;
        }

        Motorway m = new Motorway(_persistence.Fields[coord.X][coord.Y], int.MaxValue, dir)
        {
            IsCurved = isCurved,
            Quadrant = q,
        };

        if ((roads.Count == 3 || roads.Count == 4) && intersection is not null)
            m.AddIntersection(intersection);
        
        //TODO refresh neighbour roads' states
    }

    public void Place(Coordinate coord, Buildable buildable)
    {

        // 1. Épület méretének lekérése
        int width = buildable.area.Width;
        int height = buildable.area.Height;
        List<Land> targetLands = new List<Land>();
        float totalForestFactor = 0;
        (IResource?, int) resource = (null, 0);

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

        //if (cost > GetBudget()) return;
        if (buildable is Decoration decor)
        {
            resource = decor.costResource;
            if (_persistence.Storage[decor.costResource.resource] < decor.costResource.cost)
            {
                // hiba dobása? nincs elég nyersanyag
                return;
            }
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
        
        if (buildable is Decoration decoration)
        {
            _persistence.Storage[resource.Item1!] -= resource.Item2;
            OnResourceChanged(resource.Item1!, _persistence.Storage[resource.Item1!]);

            _persistence.CurrentMood += decoration.giveMood;
            OnMoodChanged(_persistence.CurrentMood);
        }
        else if (buildable is IResidentialBuilding residential)
        {
            // N�pess�g n�vel�se
            _persistence.Storage[HumanResource.Instance()] += residential.givePeople;
            OnResourceChanged(HumanResource.Instance(), _persistence.Storage[HumanResource.Instance()]);

            // Hangulat csökkentése
            _persistence.CurrentMood += residential.AffectMood;
            OnMoodChanged(_persistence.CurrentMood);
        }

        // Minden megváltozott mezőt elküldünk a View-nak
        foreach (var land in targetLands)
        {
            OnFieldsUpdated(land);
        }
    }


    public void PlaceVehicle(Coordinate coord, Vehicle<IResource> vehicle)
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

    public Field GetField(int x, int y)
    {
        return _persistence.Fields[x][y];
    }

    public Field GetField(Coordinate coords) => GetField(coords.X, coords.Y);

    public int GetMood() // should be a property
    {
        return _persistence.CurrentMood;
    }

    public int GetResourceCount(IResource resource)
    {
        return _persistence.Storage.ContainsKey(resource) ? _persistence.Storage[resource] : 0;
    }
    // id�ig �j

    public void AddVehicle(Vehicle<IResource> vehicle)
    {
    }

    public void Demolish(Coordinate coord)
    {
        // if (/*undemolishable*/)
        //     throw new Exception("ejnye-bejnye!");
        Buildable? onField = GetField(coord).Buildable;
        Field field = GetField(coord);

        Field source = GetField(new Coordinate(coord.X - field.RelativeX, coord.Y - field.RelativeY));
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
        // Megkeressük az összes olyan osztályt, ami a GoodsVehicle-ből származik
        // Mivel a GoodsVehicle generikus, a Type-szintű ellenőrzés kicsit más
        string targetNamespace = typeof(CSDB_UtopiaModel.Model.Model).Namespace;

        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        (t.IsAssignableTo(typeof(IVehicle))) && // Minden ami jármű
                        !t.IsAssignableTo(typeof(PassengerVehicle)) && // De nem utas-szállító
                        t.Namespace == targetNamespace)
            .ToList();
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

    //Szükséges új játék kezdetekor hogy a legfrissebb persistence objektum feliratkozzon az eseményekre
    private void RegisterEvents()
    {
        // Mindig a jelenlegi _persistence objektumra iratkozunk fel
        _persistence.GameOver += (_, e) => GameOver?.Invoke(this, e);
    }

    protected virtual void OnResourceChanged(IResource resource, int newValue)
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
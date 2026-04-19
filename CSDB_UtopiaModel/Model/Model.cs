using System.Reflection;
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Model : ITickable
{

    private TimeControl _timeControl;
    private Persistence.Persistence _persistence;
    private uint _totalSeconds = 0; // Az eltelt összes másodperc

    public EventHandler<EventArgs>? GameTicked;
    public EventHandler<FieldEventArgs>? FieldsUpdated;
    public EventHandler<EventArgs>? BudgetChanged;
    public EventHandler<ResourceChangedEventArgs>? ResourceChanged;
    public EventHandler<MoodChangedEventArgs>? MoodChanged;
    public EventHandler<LogEventArgs>? NewLog;
    public EventHandler<EventArgs>? NewGame;
    public EventHandler<EventArgs>? GameOver;
    public EventHandler<EventArgs>? DateChanged;
    private Map _map;

    public Map GetMap() => _map;

    public Model(int width, int height)
    {
        _persistence = new Persistence.Persistence(width, height, true);
        _map = new Map(new HashSet<Coordinate>());
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
        uint hours = _totalSeconds / 3600;
        uint minutes = (_totalSeconds % 3600) / 60;
        uint seconds = _totalSeconds % 60;
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

    public bool PlaceRoad(Coordinate coord)
    {
        (bool IsCurved, int Quadrant, IDirection Direction, Intersection? Intersection) roadState;

        try
        {
            roadState = DetermineRoadState(coord);
        }
        catch (InvalidOperationException)
        {
            return false;
        }

        Motorway newborn = new(_persistence.Fields[coord.X][coord.Y], int.MaxValue, Up.Instance())
        {
            Direction = roadState.Direction,
            IsCurved = roadState.IsCurved,
            Quadrant = roadState.Quadrant,
            Intersection = roadState.Intersection,
        };

        if (!Place(coord, newborn)) return false;

        if (RefreshNeighbouringRoads(coord))
        {
            _map.BuildRoad(coord);
            return true;

        }

        // if the refreshing was not successful
        Demolish(coord);
        _persistence.Budget += newborn.placementCost;
        return false;
    }

    public bool Place(Coordinate coord, Buildable buildable)
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
                if (coord.X + i >= _persistence.Width || coord.Y + j >= _persistence.Height) return false;

                Field f = _persistence.Fields[coord.X + i][coord.Y + j];

                // Csak szabad Land mezőre építhetünk
                if (f is Land land && !land.HasBuildable)
                {
                    targetLands.Add(land);
                    totalForestFactor += 0.05f * land.LevelOfForest;
                }
                else return false; // Ha csak egyetlen mező is foglalt vagy nem Land, megállunk
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
                return false;
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
        else if (buildable is Stop stop)
        {
            _map.BuildRoad(stop.Owner.Coordinates);
        }

        // Minden megváltozott mezőt elküldünk a View-nak
        foreach (var land in targetLands)
        {
            OnFieldsUpdated(land);
        }

        return true;
    }

    public void PlaceVehicle(Coordinate start, Coordinate end, Vehicle<IResource> vehicle)
    {
        if (_persistence.Fields[end.X][end.Y].Buildable is not Stop stop)
            throw new InvalidOperationException("You can only place a vehicle to a road");

        try
        {
            vehicle.AssignNewPath(start, end);
            _persistence.VehiclesOnMap.Add(vehicle);
            
            _persistence.Budget -= vehicle.placementCost;
            BudgetChanged?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception e)
        {
            
        }
        
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

    public HashSet<Garage> ListGarages()
    {
        return _persistence.Garages.ToHashSet();
    }

    public int GetResourceCount(IResource resource)
    {
        return _persistence.Storage.ContainsKey(resource) ? _persistence.Storage[resource] : 0;
    }
    // id�ig �j

    public void AddVehicle(Vehicle<IResource> vehicle)
    {
        _persistence.VehiclesOnMap.Add(vehicle);
        
    }

    public void Demolish(Coordinate coord)
    {
        // if (/*undemolishable*/)
        //     throw new Exception("ejnye-bejnye!");
        Buildable? onField = GetField(coord).Buildable;
        Field field = GetField(coord);
        Field source = GetField(new Coordinate(coord.X - field.RelativeX, coord.Y - field.RelativeY));

        if (onField is null) return;

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

        if (onField is Road)
        {
            RefreshNeighbouringRoads(coord);
            _map.DeleteRoad(coord);
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

    private (bool IsCurved, int Quadrant, IDirection Direction, Intersection? Intersection) DetermineRoadState(
        Coordinate coord)
    {
        int tmp, roadCount = 0;
        Field?[] roads = [null, null, null, null];
        IDirection[] roadDirections = [Up.Instance(), Right.Instance(), Down.Instance(), Left.Instance()];

        for (int i = 0; i < 4; i++)
        {
            IDirection d = roadDirections[i];
            try
            {
                Field field = GetField(coord.Step(d));
                if (field.Buildable is Road)
                {
                    roadCount++;
                    roads[i] = field;
                }
                
            }
            catch(Exception e){}
        }
        
        bool isCurved = false;
        int q = 0;
        IDirection dir = Up.Instance();
        Intersection? intersection = null;
        Field f = GetField(coord);
        switch (roadCount)
        {
            case 0: // should be also included in case 'default' if we want to build only non-separated roads
                break;
            case 1:
                // Amerre a szomszéd van, arra nézzen
                if (roads[0] is not null) dir = Up.Instance();
                else if (roads[1] is not null) dir = Right.Instance();
                else if (roads[2] is not null) dir = Down.Instance();
                else if (roads[3] is not null) dir = Left.Instance();
                break;

            case 2:
                // Szemközti szomszédok -> EGYENES
                if ((roads[0] is not null) && (roads[2] is not null))
                {
                    dir = Up.Instance();
                    isCurved = false;
                } // Függőleges
                else if (roads[1] is not null && roads[3] is not null)
                {
                    dir = Right.Instance();
                    isCurved = false;
                } // Vízszintes
                else
                {
                    // Egymás melletti szomszédok -> KANYAR
                    isCurved = true;
                    if (roads[0] is not null && roads[1] is not null) q = 1; // Fent + Jobb
                    else if (roads[0] is not null && roads[3] is not null) q = 2; // Fent + Bal
                    else if (roads[2] is not null && roads[3] is not null) q = 3; // Lent + Bal
                    else if (roads[2] is not null && roads[1] is not null) q = 4; // Lent + Jobb
                }

                break;
            case 3:
                if (HasNeighbouringIntersection(coord))
                    throw new InvalidOperationException("can't place two intersections next to each other");

                isCurved = false;
                q = 0;

                // Megkeressük, melyik irányban NINCS út a 4 közül
                // A 'dir' változó fogja tárolni a T-elágazás irányadó irányát
                if (roads[2] is null) // Nincs út LENT
                    dir = Up.Instance(); // A T szára felfelé mutat
                else if (roads[3] is null) // Nincs út BALRA
                    dir = Right.Instance();
                else if (roads[0] is null) // Nincs út FENT
                    dir = Down.Instance();
                else if (roads[1] is null) // Nincs út JOBBRA
                    dir = Left.Instance();

                intersection = new ThreeWayIntersection(f, dir);

                break;
            case 4:
                if (HasNeighbouringIntersection(coord))
                    throw new InvalidOperationException("can't place two intersections next to each other");

                isCurved = false;
                intersection = new FourWayIntersection(f);
                break;
            default:
                throw new Exception();
        }

        return (isCurved, q, dir, intersection);
    }

    private bool HasNeighbouringIntersection(Coordinate coord)
    {
        for (int i = -1; i <= 1; i++)
        {
            int newx = coord.X + i;
            if (i == 0 || !(0 <= newx && newx < _persistence.Fields.Count))
                continue;
            for (int k = -1; k <= 1; k++)
            {
                int newy = coord.Y + k;
                if (k == 0 || !(0 <= newy && newy < _persistence.Fields[newx].Count))
                    continue;

                if (_persistence.Fields[newx][newy].Buildable is Motorway { HasIntersection: true })
                    return true;
            }
        }

        return false;
    }

    private bool RefreshNeighbouringRoads(Coordinate coord)
    {
        const int size = 3;

        (bool IsCurved, int Quadrant, IDirection Direction, Intersection? Intersection)?[,] states =
            new (bool, int, IDirection, Intersection? )?[size, size];

        for (int i = 0; i < size; i++)
        for (int k = 0; k < size; k++)
            states[i, k] = null;

        for (int i = -1; i <= 1; i++)
        {
            int newx = coord.X + i;
            if (!(0 <= newx && newx < _persistence.Fields.Count))
                continue;
            for (int k = -1; k <= 1; k++)
            {
                int newy = coord.Y + k;
                if (!(i == 0 || k == 0) || !(0 <= newy && newy < _persistence.Fields[newx].Count) ||
                    _persistence.Fields[newx][newy].Buildable is not Road road)
                    continue;

                try
                {
                    states[i + 1, k + 1] = DetermineRoadState(_persistence.Fields[newx][newy].Coordinates);
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        // Refresh states if it was possible to determine them

        for (int i = -1; i <= 1; i++)
        {
            int newx = coord.X + i;
            if (!(0 <= newx && newx < _persistence.Fields.Count))
                continue;
            for (int k = -1; k <= 1; k++)
            {
                int newy = coord.Y + k;
                if (states[i + 1, k + 1] is null || !(i == 0 || k == 0) ||
                    !(0 <= newy && newy < _persistence.Fields[newx].Count) ||
                    _persistence.Fields[newx][newy].Buildable is not Road road)
                    continue;

                var roadState = states[i + 1, k + 1]!.Value;

                road.IsCurved = roadState.IsCurved;
                road.Quadrant = roadState.Quadrant;
                road.Direction = roadState.Direction;


                if (road is Motorway motorway)
                {
                    motorway.Intersection = roadState.Intersection;
                }

                OnFieldsUpdated(_persistence.Fields[newx][newy]);
            }
        }

        return true;
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
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Vehicle<R> : IVehicle where R : IResource
{
    protected int capacity;
    protected int maintenanceCost;
    protected int speed;
    public abstract int placementCost { get; }
    protected readonly int tickInterval = 1;
    protected Navigation? navigation;
    protected Navigator? navi;
    protected R? carriedResource = default;

    private Map map;
    private Model model;
    protected Coordinate position;
    protected Field? currentField;
    protected int garageLimit = 100;

    public int carriedAmount { get; set; } = 0;

    public int CarriedAmount
    {
        get => carriedAmount;
        set
        {
            if (carriedAmount > Capacity) throw new InvalidOperationException("CarriedAmount cannot be more than Capacity");
            carriedAmount = value;
        }
    }

    public string CarriedResourceName
    {
        get
        {
            // Ha éppen nem szállít semmit (carriedAmount == 0), írjunk ki mást
            if (carriedAmount == 0 || carriedResource == null)
                return "Üres";

            // Visszaadjuk az erõforrás osztályának nevét (pl. Wood, IronOre)
            return carriedResource.GetType().Name;
        }
    }

    public int Capacity => capacity;
    public int MaintenanceCost => maintenanceCost;
    public int Speed => speed;

    public virtual bool CanCarry(IResource resource)
    {
        return resource is R;
    }


    public Coordinate Position
    {
        get => position;
        protected set
        {
            position = value;
            CurrentNavigable = getRoad(position);
            currentField = model.GetField(position);


            CurrentDirection = CurrentNavigable is Road road ? road.Direction : Up.Instance();

        }
    }

    public INavigable? CurrentNavigable { get; protected set; }

    public int TraveledSinceBought { get; protected set; }
    public GoingIntention Intention { get; private set; }

    protected INavigable getRoad(Coordinate c)
    {
        Field f = model.GetField(c);
        if (f.Buildable is INavigable a)
            return a;
        throw new InvalidDataException("The specified coordinate is not navigable");
    }

    public Vehicle(Map map, Model m)
    {
        model = m;
        this.map = map;
        TimeControl tc = TimeControl.Instance();
        tc += (this, tickInterval);
        
    }
    public IDirection CurrentDirection { get; set; } = Up.Instance();

    public void AssignNewPath(Coordinate[]  stops)
    {

        CurrentNavigable?.Leave(this);
        Field? oldField = currentField;
        
        navigation = map.GetNavigation(stops);
        navi = (Navigator)navigation.GetEnumerator();

        Position = navi.Current;


        IDirection d = (CurrentNavigable is Road road) ? road.Direction : Up.Instance();

        Intention = new GoingIntention(d, d);

        
        model.FieldsUpdated?.Invoke(this, new FieldEventArgs([oldField, currentField]));
    }

    private void GoToGarage()
    {
        TraveledSinceBought = 0;
        List<Coordinate> garages = model.ListGarages().Select(g => g.Owner.Coordinates).ToList();
        Coordinate nearestGarage = map.GetNearest(Position, garages);
        navi.TemporaryStop = nearestGarage;
    }

    public int Sell() => throw new NotImplementedException();

   

    public Task Tick()
    {
        if (navi is null) return Task.CompletedTask;
        if (navi.MoveNext())
        {
            IDirection d = Position / navi.Current;

            if (!getRoad(navi.Current).TryMoveTo(d, this)) return Task.CompletedTask;

            CurrentNavigable?.Leave(this);

            Field oldField = currentField;

            Position = navi.Current;

            Intention = Intention.NewIntention(d);

            Field to = model.GetField(Position);
            currentField = to;

            if (currentField.HasBuildable && currentField.Buildable is Stop stop)
            {
                // LERAKODÁS: Ha van nálunk valami, azt lepakoljuk
                if (CarriedAmount > 0 && carriedResource != null)
                {
                    stop.Load(carriedResource, CarriedAmount);
                    CarriedAmount = 0;
                    carriedResource = default;
                }
                // FELVÉTEL: Ha üresek vagyunk, keresünk a megállóban olyat, amit elvihetünk
                else if (CarriedAmount == 0)
                {
                    var (foundResource, takenAmount) = stop.UnloadAnythingValid(this, Capacity);

                    // Csak akkor próbáljuk meg beállítani, ha kaptunk valamit ÉS az tényleg R típusú
                    if (foundResource is R validResource && takenAmount > 0)
                    {
                        this.carriedResource = validResource;
                        this.CarriedAmount = takenAmount;
                    }
                }
            }

            model.FieldsUpdated?.Invoke(this, new FieldEventArgs([oldField, to]));
            TraveledSinceBought++;

        }
        else if (TraveledSinceBought > garageLimit) GoToGarage();
        else navi.Reset();
        

        return Task.CompletedTask;
    }

    public int GetPositionInField() => throw new NotImplementedException();
}
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

    public int Capacity => capacity;
    public int MaintenanceCost => maintenanceCost;
    public int Speed => speed;

    


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
    /*
    public Vehicle(Map map, Model m, Coordinate start, Coordinate end)
    {
        TimeControl tc = TimeControl.Instance();
        tc += (this, tickInterval);
        model = m;
        this.map = map;
        Position = start;
        AssignNewPath(start, end);
    }
    */
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

            if (currentField.HasBuildable && currentField.Buildable is Stop stop && carriedResource is not null)
            {
                stop.Load(carriedResource, Capacity);
                CarriedAmount = 0;
                
                carriedAmount = stop.Unload(carriedResource, CarriedAmount);

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
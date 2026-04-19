using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Vehicle<R> : IVehicle where R : IResource
{
    protected int capacity;
    protected int maintenanceCost;
    protected int speed;
    public virtual int placementCost => 200;
    protected int tickInterval = 1;
    protected Navigation navigation;
    protected Navigator navi;
    protected Map map;
    private Model model;
    protected Coordinate position;
    protected Field currentField;
    protected int garageLimit = 1000;

    public Coordinate Position
    {
        get => position;
        protected set
        {
            position = value;
            CurrentNavigable = getRoad();
            currentField = model.GetField(position);

            
            CurrentDirection = CurrentNavigable is Road road ? road.Direction : Up.Instance();
                
        }
    }

    public INavigable CurrentNavigable { get; protected set; }
    
    public int TraveledSinceBought { get; set; }
    public GoingIntention Intention { get; private set; }

    protected INavigable getRoad()
    {
        Field f = model.GetField(Position);
        if (f.Buildable is INavigable a) 
            return a;
        throw new InvalidDataException("The specified coordinate is not navigable");
    }

    public Vehicle(Map map, Model m, Coordinate start, Coordinate end)
    {
        TimeControl tc = TimeControl.Instance();
        tc += (this, tickInterval);
        model = m;
        this.map = map;
        Position = start;
        AssignNewPath(start, end);
    }
    public IDirection CurrentDirection { get; set; } = Up.Instance();

    public void AssignNewPath(Coordinate start, Coordinate end)
    {
        navigation = map.GetNavigation(start, end);
        navi = (Navigator)navigation.GetEnumerator();

        Field oldField = currentField;
        Position = start; // A setter be�ll�tja a currentField-et is

        IDirection d = (CurrentNavigable is Road road) ? road.Direction : Up.Instance();
        
        Intention = new GoingIntention(d, d);

        navi = (Navigator)navigation.GetEnumerator();
        
        model.FieldsUpdated?.Invoke(this, new FieldEventArgs([oldField, currentField]));
    }

    public void GoToGarage()
    {
        HashSet<Garage> garages = model.ListGarages();
        Coordinate garagePos = map.GetNearest(Position, garages.Select(garage => garage.Owner.Coordinates).ToList());
        AssignNewPath(Position, garagePos);

    }
    public int Sell() => throw new NotImplementedException();

    public Task Tick()
    {
        if (navi.MoveNext())
        {
            IDirection d = Position/navi.Current;
            CurrentNavigable.Leave(this);
            Field oldField = currentField;

            Position = navi.Current;
            
            Intention = Intention.newIntention(d);
            
            CurrentNavigable.MoveTo(d, this);
            Field to = model.GetField(Position);
            
            model.FieldsUpdated?.Invoke(this, new FieldEventArgs([oldField, to]));
            currentField = to;
            TraveledSinceBought++;
            
        }
        else if (navi.Ended)
        { 
            if (TraveledSinceBought > garageLimit) GoToGarage();
            else navi.Reset();
        }
        return Task.CompletedTask;
    }
    public int GetPositionInField() => throw new NotImplementedException();
}
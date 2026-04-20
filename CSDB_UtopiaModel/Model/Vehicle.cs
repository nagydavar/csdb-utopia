using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Vehicle<R> : IVehicle where R : IResource
{
    protected int capacity;
    protected int maintenanceCost;
    protected int speed;
    protected int tickInterval;
    protected Navigation regularNavigation;
    protected Navigator regularNavi;
    protected Navigation garageNavigation;
    protected Navigator garageNavi;
    protected Navigation navigation => GoingToGarage ? garageNavigation : regularNavigation;
    protected Navigator navi => GoingToGarage ? garageNavi : regularNavi;
    protected Map map;
    private Model model;
    protected Coordinate position;
    protected Field currentField;
    protected int garageLimit = 1000;
    public bool GoingToGarage { get; protected set; }
    public bool GoingFromGarage { get; protected set; }
    public bool GoingGarage => GoingToGarage || GoingFromGarage;

    public void GoingGarageNextStep()
    {
        if (!GoingToGarage)
        {
            GoingToGarage = true;
            GoingFromGarage = false;
        }

        if (!GoingFromGarage)
        {
            GoingFromGarage = true;
            GoingToGarage = false;
        }
        else
        {
            GoingFromGarage = false;
            GoingToGarage = false;
        }
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
    
    public int TraveledSinceBought { get; set; }
    public GoingIntention? Intention { get; private set; }

    protected INavigable getRoad(Coordinate c)
    {
        Field f = model.GetField(c);
        if (f.Buildable is INavigable a) 
            return a;
        throw new InvalidDataException("The specified coordinate is not navigable");
    }

    public Vehicle(Map map, Model m)
    {
        regularNavigation = map.GetNavigation(start, end);
        regularNavi = (Navigator)navigation.GetEnumerator();
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

    public void AssignNewPath(Coordinate start, Coordinate end)
    {
        
        CurrentNavigable?.Leave(this);
        Field oldField = currentField;
        
        Position = start;
        navigation = map.GetNavigation(start, end);
        navi = (Navigator)navigation.GetEnumerator();


        IDirection d = (CurrentNavigable is Road road) ? road.Direction : Up.Instance();
        
        Intention = new GoingIntention(d, d);

        
        model.FieldsUpdated?.Invoke(this, new FieldEventArgs([oldField, currentField]));
    }

    public void GoToGarage()
    {
        GoingGarageNextStep();
        Coordinate? nextPos = null;
        if (GoingToGarage)
        {
            HashSet<Garage> garages = model.ListGarages();
            nextPos = map.GetNearest(Position, garages.Select(garage => garage.Owner.Coordinates).ToList());
        }
        else if (GoingFromGarage)
        {
            nextPos = navigation.End;
        }
        if (nextPos is not null)
        {
            garageNavigation = map.GetNavigation(Position, nextPos.Value);
            garageNavi = (Navigator)navigation.GetEnumerator();
        }

    }
    public int Sell() => throw new NotImplementedException();

    public Task Tick()
    {
        if (navi is not null && navi.MoveNext())
        {
            IDirection d = Position/navi.Current;
            
            if (!getRoad(navi.Current).TryMoveTo(d, this)) return Task.CompletedTask;
            
            CurrentNavigable.Leave(this);
            
            Field oldField = currentField;

            Position = navi.Current;
            
            Intention = Intention.newIntention(d);
            
            Field to = model.GetField(Position);
            
            model.FieldsUpdated?.Invoke(this, new FieldEventArgs([oldField, to]));
            currentField = to;
            TraveledSinceBought++;
            
        }
        else if (navi.Ended)
        {
            if (GoingGarage)
            {
                GoToGarage();
            }
            else if (TraveledSinceBought > garageLimit)
            {
                
                GoToGarage();
                TraveledSinceBought = 0;
            }
            else navi.Reset();
        }
        return Task.CompletedTask;
    }
    public int GetPositionInField() => throw new NotImplementedException();
}
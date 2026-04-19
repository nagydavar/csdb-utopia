using System.Reflection.PortableExecutable;
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Vehicle<R> : IVehicle where R : IResource
{
    protected int capacity;
    protected int maintenanceCost;
    protected int speed;
    protected int tickInterval;
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
            CurrentRoad = getRoad();
            currentField = model.GetField(position);
        }
    }

    public INavigable CurrentRoad { get; protected set; }
    
    public int TraveledSinceBought { get; set; }
    public GoingIntention Intention { get; }

    protected INavigable getRoad()
    {
        Field f = model.GetField(Position);
        if (f.Buildable is INavigable a) 
            return a;
        throw new InvalidDataException($"Cannot find road {Position}");
    }

    public Vehicle(Map map, Model m, Coordinate start, Coordinate end)
    {
        navigation = map.GetNavigation(start, end);
        navi = (Navigator)navigation.GetEnumerator();
        model = m;
        Position = start;
        TimeControl tc = TimeControl.Instance();
        tc += (this, tickInterval);
    }

    public void AssignNewPath(Coordinate start, Coordinate end)
    {
        navigation = map.GetNavigation(start, end);
        Field oldField = currentField;
        Position = new Coordinate(start.X, start.Y);
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
            
            CurrentRoad.Leave(this);
            currentField = model.GetField(Position);
            Position = navi.Current;
            
            
            CurrentRoad.MoveTo(d, this);
            Field to = model.GetField(Position);
            
            model.FieldsUpdated?.Invoke(this, new FieldEventArgs([currentField, to]));
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
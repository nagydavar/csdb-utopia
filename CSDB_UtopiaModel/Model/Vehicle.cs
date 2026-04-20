using System.Reflection.PortableExecutable;
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
        regularNavigation = map.GetNavigation(start, end);
        regularNavi = (Navigator)navigation.GetEnumerator();
        model = m;
        Position = start;
        TimeControl tc = TimeControl.Instance();
        tc += (this, tickInterval);
    }

    public void AssignNewPath(Coordinate start, Coordinate end)
    {
        regularNavigation = map.GetNavigation(start, end);
        Field oldField = currentField;
        Position = new Coordinate(start.X, start.Y);
        regularNavi = (Navigator)navigation.GetEnumerator();
        
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
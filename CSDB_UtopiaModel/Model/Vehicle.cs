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

    public Coordinate Position
    {
        get => position;
        protected set
        {
            position = value;
            CurrentRoad = getRoad();
        }
    }

    public INavigable CurrentRoad { get; protected set; }
    
    public int TraveledSinceBought { get; set; }
    //public int TimeSpentOnCurrentRoad { get; set; }
    public GoingIntention Intention { get; }

    protected INavigable getRoad()
    {
        Field f = model.GetField(Position);
        if (f.Buildable is INavigable a) 
            return a;
        throw new Exception($"Cannot find road {Position}");
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
    
    protected bool TimeToGoToGarage() => throw new NotImplementedException();
    protected LinkedList<Field> CalculatePathToNearestGarage() => throw new NotImplementedException();
    public void GoToGarage() => throw new NotImplementedException();
    public void GoBackToPath() => throw new NotImplementedException();
    public int Sell() => throw new NotImplementedException();

    public Task Tick()
    {
        if (navi.MoveNext())
        {
            IDirection d = Position/navi.Current;
            
            CurrentRoad.Leave(this);
            Field from = model.GetField(Position);
            
            Position = navi.Current;
            
            CurrentRoad.MoveTo(d, this);
            Field to = model.GetField(Position);
            
            model.FieldsUpdated?.Invoke(this, new FieldEventArgs([from, to]));

            TraveledSinceBought++;
        }
        else if (navi.Ended)
        {
            navi.Reset();
        }
        return Task.CompletedTask;
    }
    public int GetPositionInField() => throw new NotImplementedException();
}
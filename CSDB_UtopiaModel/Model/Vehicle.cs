using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Vehicle<R> : IVehicle where R : IResource
{
    protected int capacity;
    protected int maintenanceCost;
    protected int speed;
    protected Navigator navi;
    protected LinkedList<Field>? PathToGarage;

    public Field Position { get; set; }
    public int TimeSinceBought { get; set; }
    public int TimeSpentOnCurrentRoad { get; set; }
    public int ThresholdToMove { get; set; }
    public GoingIntention Intention { get; }

    public Vehicle(Field _, int a, int b, int c, Navigator d) => throw new NotImplementedException();
    
    protected bool TimeToGoToGarage() => throw new NotImplementedException();
    protected LinkedList<Field> CalculatePathToNearestGarage() => throw new NotImplementedException();
    public void GoToGarage() => throw new NotImplementedException();
    public void GoBackToPath() => throw new NotImplementedException();
    public int Sell() => throw new NotImplementedException();
    public Task Tick() => throw new NotImplementedException();
    public int GetPositionInField() => throw new NotImplementedException();
}
namespace CSDB_UtopiaModel.Model;

public class Navigator : IEnumerator<Coordinate>
{
    private Coordinate start;
    private Coordinate end;
    private bool reverse = false;
    public Coordinate Destination => reverse ? start : end;
    private Map map;
    private Coordinate current;

    public Coordinate Current => current;

    object System.Collections.IEnumerator.Current => Current; // explicit interface implementation, not to be modified

    public bool MoveNext()
    {
        if (current == Destination) return false;
        Coordinate? step = map.Step(Current, Destination);
        if (step is not null) current = step.Value;
        return step is not null;
    }


    public void Dispose(){}

    public void Reset() => current = start;

    public Navigator(Coordinate start, Coordinate end, Map map)
    {
        this.start = start;
        this.end = end;
        this.map = map;
        current =  start;
    }
    
}
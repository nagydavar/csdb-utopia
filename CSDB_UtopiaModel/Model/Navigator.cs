namespace CSDB_UtopiaModel.Model;

public class Navigator : IEnumerator<Coordinate>
{
    private List<Coordinate> stops;
    private int stopIndex = 1;
    private Coordinate Destination => TemporaryStop.HasValue ? TemporaryStop.Value : stops[stopIndex];
    private int StepIndDiff => IsReversed ? -1 : 1;

    private bool IsReversed { get; set; } = false;
    private Map map;
    private Coordinate current;
    private Coordinate? temporaryStop = null;
    public Coordinate? TemporaryStop { get => temporaryStop;
        set
        {
            if (temporaryStop is null) temporaryStop = value;
        }
    }


    public Coordinate Current => current;

    object System.Collections.IEnumerator.Current => Current; // explicit interface implementation, not to be modified

    private void StepNextStop()
    {
        if (TemporaryStop.HasValue) temporaryStop = null;
        else stopIndex += StepIndDiff;

    }

    public bool MoveNext()
    {

        if (EndedSection) StepNextStop();
        if (Ended) return false;

        Coordinate? step = map.Step(Current, Destination);
        if (step is not null) current = step.Value;
        return step is not null;
    }

    private bool EndedSection => Ended || Current == Destination;
    public void Dispose(){}

    public void Reset()
    {
        if (!Ended) return;
        IsReversed = !IsReversed;
        if (IsReversed) stopIndex = stops.Count - 2;
        else stopIndex = 1;
    }

    public bool Ended => !TemporaryStop.HasValue && (stops.Count <= stopIndex || stopIndex < 0);
    

    public Navigator(Coordinate start, Coordinate end, Map map) : this([start, end], map){}
    public Navigator(IList<Coordinate> stops, Map map)
    {
        if (stops.Count < 2) throw new InvalidOperationException("A path should have at least two stops");
        current = stops[0];
        this.stops = stops.ToList();
        this.map = map;

    }
}
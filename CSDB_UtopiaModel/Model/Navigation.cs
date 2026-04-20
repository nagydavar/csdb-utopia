using System.Collections;

namespace CSDB_UtopiaModel.Model;

public class Navigation : IEnumerable<Coordinate>
{
    public Coordinate Start { get; protected set; }
    public Coordinate End { get; protected set; }
    private Map map;

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    IEnumerator<Coordinate> IEnumerable<Coordinate>.GetEnumerator() => GetEnumerator();

    public IEnumerator<Coordinate> GetEnumerator()
    {
        return new Navigator(Start, End, map);
    }

    public Navigation(Coordinate start, Coordinate end, Map map)
    {
        this.Start = start;
        this.End = end;
        this.map = map;
    }
}
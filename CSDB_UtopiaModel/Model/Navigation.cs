using System.Collections;

namespace CSDB_UtopiaModel.Model;

public class Navigation : IEnumerable<Coordinate>
{
    private Coordinate start;
    private Coordinate end;
    private Map map;

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<Coordinate> GetEnumerator()
    {
        return new Navigator(start, end, map);
    }

    public Navigation(Coordinate start, Coordinate end, Map map)
    {
        this.start = start;
        this.end = end;
        this.map = map;
    }
}
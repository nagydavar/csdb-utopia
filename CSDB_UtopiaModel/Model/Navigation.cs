using System.Collections;

namespace CSDB_UtopiaModel.Model;

public class Navigation : IEnumerable<Coordinate>
{
    private IList<Coordinate> stops;
    private Map map;

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    IEnumerator<Coordinate> IEnumerable<Coordinate>.GetEnumerator() => GetEnumerator();

    public IEnumerator<Coordinate> GetEnumerator()
    {
        return new Navigator(stops, map);
    }
    public Navigation(Coordinate start, Coordinate end, Map map):this([start, end], map){}
    public Navigation(Coordinate[] stops, Map map)
    {
        this.stops = new List<Coordinate>(stops);
        this.map = map;
    }
}
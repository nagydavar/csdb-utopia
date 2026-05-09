
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

[TestClass]
public class TestModel
{
    private Model? m = null;
    private static readonly List<Coordinate> roads = new()
    {
        new(0, 0), new(0, 1), new(0, 2), new(1, 2),
        new(2, 2), new(2, 1), new(2, 0), new(1, 0)
    };


    [TestInitialize]
    public void TestInitialize()
    {
        TimeControl.ResetInstance();
        m = new Model(10, 10);
    }
    [TestMethod]
    public void TestPlaceRoad()
    {
        if (m is null) return;
        foreach (Coordinate coord in roads)
        {
            m.PlaceRoad(coord);
            Field f = m.GetField(coord);
            if (f is Land)
            {
                Assert.IsNotNull(f.Buildable);
                Assert.IsInstanceOfType<Road>(f.Buildable);
                Road r = (Road)f.Buildable;
                Assert.IsNull(r.RightSide);
                Assert.IsNull(r.LeftSide);
            }
        }
    }
    [TestMethod]
    public void TestPlace()
    {
        
        if (m is null) return;
        foreach (Coordinate coord in roads)
        {
            Field f = m.GetField(coord);
            m.Place(coord, new DetachedHouse(f));
            if (f is Land)
            {
                Assert.IsNotNull(f.Buildable);
                Assert.IsInstanceOfType<DetachedHouse>(f.Buildable);
            }
        }
    }
    
    
    [TestMethod]
    public void TestDemolishRoad()
    {
        foreach (Coordinate coord in roads)
        {
            m.PlaceRoad(coord);
            Field f = m.GetField(coord);
            if (f is Land)
            {
                Assert.IsNotNull(f.Buildable);
                Assert.IsInstanceOfType<Road>(f.Buildable);
            }
        }
        
        m.Demolish(roads[0]);
        Assert.IsNull(m.GetField(roads[0]).Buildable);
        
        Assert.IsNull(m.GetMap().Step(roads[0], roads[4]));
        
    }
    
}
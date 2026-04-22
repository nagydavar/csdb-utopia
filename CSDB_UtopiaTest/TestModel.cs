
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

[TestClass]
public sealed class TestModel
{
    private Model? m = null;
    private List<Coordinate> roads = new();
    
        
    [TestInitialize]
    public void TestInitialize()
    {
        try
        {
            m = new Model(10, 10);
            
            roads.Clear();
            roads.Add(new Coordinate(0, 0));//0
            roads.Add(new Coordinate(0, 1));//1
            roads.Add(new Coordinate(0, 2));//2
            roads.Add(new Coordinate(1, 2));//3
            roads.Add(new Coordinate(2, 2));//4
            roads.Add(new Coordinate(2, 1));//5
            roads.Add(new Coordinate(2, 0));//6
            roads.Add(new Coordinate(1, 0));//7
        }
        catch (Exception e)
        {
            
        }
        
        
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
using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaTest;



[TestClass]
public class NavigationTest
{
    private List<Coordinate> roads = new();
    private Map m = null!;
    [TestInitialize]
    public void Initialize()
    {
        roads.Clear();
        roads.Add(new Coordinate(0, 0));//0
        roads.Add(new Coordinate(0, 1));//1
        roads.Add(new Coordinate(0, 2));//2
        roads.Add(new Coordinate(1, 2));//3
        roads.Add(new Coordinate(2, 2));//4
        roads.Add(new Coordinate(2, 1));//5
        roads.Add(new Coordinate(2, 0));//6
        roads.Add(new Coordinate(1, 0));//7
        
        m = new Map(roads.ToHashSet());
    }

    [TestMethod]
    public void TestCreation()
    {
        Navigation n = m.GetNavigation(roads[0], roads[5]);
        List<Coordinate> path = new List<Coordinate>(n);
        Assert.HasCount(3,  path);
        
        
    }
    
    [TestMethod]
    public void TestCreation2()
    {
        Navigation n = m.GetNavigation(roads[0], roads[5]);
        List<Coordinate> path = new List<Coordinate>(n);
        Assert.AreEqual(roads[7], path[0]);
        Assert.AreEqual(roads[6], path[1]);
        Assert.AreEqual(roads[5], path[2]);
    }
    
    
    [TestMethod]
    public void TestDeleteRoad()
    {
        m.DeleteRoad(roads[7]);
        m.DeleteRoad(roads[3]);
        Navigation n = m.GetNavigation(roads[0], roads[4]);
        List<Coordinate> path = new List<Coordinate>(n);
        Assert.HasCount(0, path);
    }
}
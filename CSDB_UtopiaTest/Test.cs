using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaTest;

[TestClass]
public sealed class Test
{
    [TestMethod]
    public void MapTest()
    {
        HashSet<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(1, 0));
        Map m = new Map(roads);
        Coordinate? step = m.Step(new Coordinate(1, 0), new Coordinate(0, 1));
        Coordinate? step2 = m.Step(new Coordinate(0, 0), new Coordinate(0, 1));
        Assert.IsTrue(step.HasValue);
        
        Assert.AreEqual(new Coordinate(0, 0), step.Value);

        Assert.IsTrue(step2.HasValue);
        
        Assert.AreEqual(new Coordinate(0, 1), step2.Value);

    }
    [TestMethod]
    public void MapTest2()
    {
        List<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(1, 0));
        Map m = new Map(roads.ToHashSet());
        Navigation n = m.GetNavigation(roads[3], roads[2]);
        List<Coordinate> path = new List<Coordinate>(n);
        
        Assert.AreEqual(3, path.Count);
        Assert.AreEqual(roads[0], path[0]);
        Assert.AreEqual(roads[1], path[1]);
        Assert.AreEqual(roads[2], path[2]);

    }
    [TestMethod]
    public void MapTestNoPath()
    {
        List<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(2, 0));
        Map m = new Map(roads.ToHashSet());
        Navigation n = m.GetNavigation(roads[3], roads[2]);
        List<Coordinate> path = new List<Coordinate>(n);
        
        Assert.AreEqual(0, path.Count);

    }
    [TestMethod]
    public void MapTestBuildRoad()
    {
        List<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(2, 0));
        Map m = new Map(roads.ToHashSet());
        m.BuildRoad(new Coordinate(1, 0));
        Navigation n = m.GetNavigation(roads[3], roads[2]);
        List<Coordinate> path = new List<Coordinate>(n);
        
        Assert.AreEqual(4, path.Count);

    }
    [TestMethod]
    public void MapTestBuildRoadNoPath()
    {
        List<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(2, 0));
        Map m = new Map(roads.ToHashSet());
        m.BuildRoad(new Coordinate(3, 0));
        Navigation n = m.GetNavigation(roads[3], roads[2]);
        List<Coordinate> path = new List<Coordinate>(n);
        
        Assert.AreEqual(0, path.Count);

    }
    
    
    [TestMethod]
    public void MapTestDeleteRoadNoPath()
    {
        List<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(2, 0));
        Map m = new Map(roads.ToHashSet());
        m.DeleteRoad(new Coordinate(0, 0));
        Navigation n = m.GetNavigation(roads[3], roads[2]);
        List<Coordinate> path = new List<Coordinate>(n);
        
        Assert.AreEqual(0, path.Count);

    }
        
    [TestMethod]
    public void MapTestDeleteRoad()
    {
        List<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(2, 0));
        Map m = new Map(roads.ToHashSet());
        m.DeleteRoad(new Coordinate(2, 0));
        Navigation n = m.GetNavigation(roads[0], roads[2]);
        List<Coordinate> path = new List<Coordinate>(n);
        
        Assert.AreEqual(2, path.Count);

    }
    
           
    [TestMethod]
    public void MapTestDeleteRoadBuildRoad2()
    {
        List<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(1, 0));
        Map m = new Map(roads.ToHashSet());
        m.DeleteRoad(new Coordinate(0, 1));
        m.BuildRoad(new Coordinate(0, 1));
        Navigation n = m.GetNavigation(roads[3], roads[2]);
        List<Coordinate> path = new List<Coordinate>(n);
        
        Assert.AreEqual(3, path.Count);

    }
    
    [TestMethod]
    public void MapTestDeleteRoad2()
    {
        List<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(1, 2));
        roads.Add(new Coordinate(2, 2));
        roads.Add(new Coordinate(2, 1));
        roads.Add(new Coordinate(2, 0));
        roads.Add(new Coordinate(1, 0));
        
        Map m = new Map(roads.ToHashSet());
        m.DeleteRoad(new Coordinate(0, 1));
        Navigation n = m.GetNavigation(roads[0], roads[4]);
        List<Coordinate> path = new List<Coordinate>(n);
        
        Assert.AreNotEqual(0, path.Count);

    }
    [TestMethod]
    public void MapTestDeleteRoad3()
    {
        List<Coordinate> roads = new();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(1, 2));
        roads.Add(new Coordinate(2, 2));
        roads.Add(new Coordinate(2, 1));
        roads.Add(new Coordinate(2, 0));
        roads.Add(new Coordinate(1, 0));
        
        Map m = new Map(roads.ToHashSet());
        m.DeleteRoad(new Coordinate(0, 1));
        m.DeleteRoad(new Coordinate(1, 0));
        Navigation n = m.GetNavigation(roads[0], roads[4]);
        List<Coordinate> path = new List<Coordinate>(n);
        
        Assert.AreEqual(0, path.Count);

    }
}
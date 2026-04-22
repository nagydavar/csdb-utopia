using CSDB_UtopiaModel.Model;
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaTest;

public class TestVehicle
{
    private Model m = null!;
    private List<Coordinate> roads = new();
    
        
    [TestInitialize]
    public void TestInitialize()
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
    [TestMethod]
    public void TestPlaceRoad()
    {
        Bus b = new Bus(m.GetMap(), m);
        Bus c = new Bus(m.GetMap(), m);
        foreach (Coordinate coord in roads)
        {
            m.PlaceRoad(coord);
            Field f = m.GetField(coord);
            if (f is Land)
            {
                Assert.IsNotNull(f.Buildable);
                Assert.IsInstanceOfType<Road>(f.Buildable);
                Road r = (Road)f.Buildable;
                r.RightSide = b;
                Assert.Throws<InvalidOperationException>(() => r.RightSide = c);
            }
        }
    }
}
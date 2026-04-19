namespace CSDB_UtopiaTest;
using CSDB_UtopiaModel.Model;
[TestClass]
public sealed class VehicleTest
{
    private Map map = null!;
    private Model m = null!;
    
    List<Coordinate> roads = new();
    [TestInitialize]
    public void MapPrepare()
    {
        roads.Clear();
        roads.Add(new Coordinate(0, 0));
        roads.Add(new Coordinate(0, 1));
        roads.Add(new Coordinate(0, 2));
        roads.Add(new Coordinate(1, 2));
        roads.Add(new Coordinate(2, 2));
        roads.Add(new Coordinate(2, 1));
        roads.Add(new Coordinate(2, 0));
        roads.Add(new Coordinate(1, 0));
        //map = new Map(roads.ToHashSet());
        m = new Model(4, 4);
    }
    [TestMethod]
    public void Ctor()
    {

    }
}
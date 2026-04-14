using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class ThreeWayIntersection: Intersection
{
    //A T-nek szára adja meg a Direction
    public ThreeWayIntersection(Field f, IDirection d): base(f, d) {} 
}
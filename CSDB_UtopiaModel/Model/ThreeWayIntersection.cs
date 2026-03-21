using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class ThreeWayIntersection: Intersection
{
    //A T-nek szára adja meg a Direction
    public ThreeWayIntersection(Field f, Direction d): base(f, d) {} 
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class FourWayIntersection: Intersection
{
    public FourWayIntersection(Field f): base(f, UP.Instance()) {} 
}
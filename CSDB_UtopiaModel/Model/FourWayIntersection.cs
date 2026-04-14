using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class FourWayIntersection: Intersection
{
    public FourWayIntersection(Field f): base(f, Up.Instance()) {} 
}
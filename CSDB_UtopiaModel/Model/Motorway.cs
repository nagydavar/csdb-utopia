using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Motorway : Road
{
        private Intersection? intersection;
        public bool HasIntersection { get => intersection is not null; }
        public Motorway(Field f, int maxSpeed, IDirection d): base(f, maxSpeed, d) {}
        
        public Intersection? GetIntersection() => intersection;    

        public void AddIntersection(Intersection i)
        {
                intersection = i;
        }
        
};

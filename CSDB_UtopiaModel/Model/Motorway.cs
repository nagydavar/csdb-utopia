using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Motorway : Road
{
        private Intersection? intersection;
        public bool HasIntersection { get => intersection is not null; }
        public Motorway(Field f, int maxSpeed, Direction d): base(f, maxSpeed, d) {}

        public void AddIntersection(Intersection i)
        {
                intersection = i;
        }
        
};

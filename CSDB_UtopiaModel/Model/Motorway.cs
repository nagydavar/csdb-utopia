using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Motorway : Road
{ 
        private Intersection? _intersection;

        public bool HasIntersection
        {
                get => _intersection is not null;
        }

        public Motorway(Field f, int maxSpeed, Direction d) : base(f, maxSpeed, d)
        {
        }

        public void AddIntersection(Intersection i)
        { 
                _intersection = i;
        }
}
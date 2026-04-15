using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Motorway : Road
{
        private Intersection? _intersection;

        public bool HasIntersection
        {
                get => _intersection is not null;
        }

        public Motorway(Field f, int maxSpeed, IDirection d) : base(f, maxSpeed, d)
        {
        }

        public Intersection? GetIntersection() => _intersection;

        public void AddIntersection(Intersection i)
        {
                if (HasIntersection)
                        throw new InvalidOperationException("The road already has an intersection");
                _intersection = i;
        }
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Motorway : Road
{
        public Intersection? Intersection { get; internal set; }

        public bool HasIntersection
        {
                get => Intersection is not null;
        }

        public Motorway(Field f, int maxSpeed, IDirection d) : base(f, maxSpeed, d)
        {
        }

        public Intersection? GetIntersection() => Intersection;

        internal void AddIntersection(Intersection i)
        {
                Intersection = i;
        }
}
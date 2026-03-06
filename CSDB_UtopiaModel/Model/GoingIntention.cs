using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class GoingIntention {
        public Direction from;
        public Direction to;
        public Set<IntersectionSegment> TouchedSegments();
        public bool Crosses(GoingIntention);
        public void GoingIntention(Direction, Direction);
        public override bool equals(GoingIntetion);
        public override int compare(GoingIntetion);
    };

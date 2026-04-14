using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class GoingIntention : IComparable<GoingIntention>
{
    public Direction From { get; set; }
    public Direction To { get; set; }

    public GoingIntention(Direction from, Direction to)
    {
        From = from;
        To = to;
    }

    public HashSet<IntersectionSegment> TouchedSegments() => throw new NotImplementedException();
    public bool Crosses(GoingIntention _) => throw new NotImplementedException();
    public override bool Equals(object? obj) => throw new NotImplementedException();
    public override int GetHashCode() => throw new NotImplementedException();
    public int CompareTo(GoingIntention? _) => throw new NotImplementedException();

    public static bool operator ==(GoingIntention? lhs, GoingIntention? rhs) => lhs is not null && lhs.Equals(rhs);

    public static bool operator !=(GoingIntention? lhs, GoingIntention? rhs) => !(lhs == rhs);
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class GoingIntention : IComparable<GoingIntention>
{
    public IDirection From { get; set; }
    public IDirection To { get; set; }

    public GoingIntention NewIntention(IDirection d)
    {
        return new GoingIntention(To, d);
    }
        

    public GoingIntention(IDirection from, IDirection to)
    {
        From = from;
        To = to;
    }

    public GoingIntention newIntention(IDirection newTo)
    {
        return new GoingIntention(To, newTo);
    }

    public HashSet<IntersectionSegment> TouchedSegments() => throw new NotImplementedException();
    public bool Crosses(GoingIntention _) => throw new NotImplementedException();
    public override bool Equals(object? obj) => throw new NotImplementedException();
    public override int GetHashCode() => throw new NotImplementedException();
    public int CompareTo(GoingIntention? _) => throw new NotImplementedException();

    public static bool operator ==(GoingIntention? lhs, GoingIntention? rhs) => lhs is not null && lhs.Equals(rhs);

    public static bool operator !=(GoingIntention? lhs, GoingIntention? rhs) => !(lhs == rhs);
}
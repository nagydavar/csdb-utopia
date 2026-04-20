using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class GoingIntention : IComparable<GoingIntention>
{
    public IDirection From { get; set; }
    public IDirection To { get; set; }

    public GoingIntention(IDirection from, IDirection to)
    {
        From = from;
        To = to;
    }

    public HashSet<IntersectionSegment> TouchedSegments()
    {
        HashSet<IntersectionSegment> set = new();

        switch (From)
        {
            case Down:
                set.Add(IntersectionSegment.LowerRight);
                switch (To)
                {
                    case Up:
                        set.Add(IntersectionSegment.UpperRight);
                        break;
                    case Left:
                        set.Add(IntersectionSegment.UpperLeft);
                        break;
                }

                break;
            case Right:
                set.Add(IntersectionSegment.UpperRight);
                switch (To)
                {
                    case Left:
                        set.Add(IntersectionSegment.UpperLeft);
                        break;
                    case Down:
                        set.Add(IntersectionSegment.LowerLeft);
                        break;
                }

                break;
            case Up:
                set.Add(IntersectionSegment.UpperLeft);
                switch (To)
                {
                    case Down:
                        set.Add(IntersectionSegment.LowerLeft);
                        break;
                    case Right:
                        set.Add(IntersectionSegment.LowerRight);
                        break;
                }

                break;
            case Left:
                set.Add(IntersectionSegment.LowerLeft);
                switch (To)
                {
                    case Right:
                        set.Add(IntersectionSegment.LowerRight);
                        break;
                    case Up:
                        set.Add(IntersectionSegment.UpperRight);
                        break;
                }

                break;
        }

        return set;
    }

    public bool Crosses(GoingIntention other) => TouchedSegments().Intersect(other.TouchedSegments()).Any();

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not GoingIntention goingIntention) return false;
        return From == goingIntention.From && To == goingIntention.To;
    }

    public override int GetHashCode() => HashCode.Combine(From, To);

    public int CompareTo(GoingIntention? other)
    {
        int defaultValue = -1;

        if (other is null) return 1;
        if (Equals(other) || !Crosses(other)) return 0;

        return other.From.FromPerspectiveOf(From) switch
        {
            Right => -1,
            Left => 1,
            Up => other.To.FromPerspectiveOf(From) switch
            {
                Down or Left => -1,
                Right => 1,
                _ => defaultValue
            },
            _ => defaultValue
        };
    }

    public static bool operator ==(GoingIntention? lhs, GoingIntention? rhs) => lhs is not null && lhs.Equals(rhs);

    public static bool operator !=(GoingIntention? lhs, GoingIntention? rhs) => !(lhs == rhs);
}
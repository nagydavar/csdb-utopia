using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

[System.Diagnostics.DebuggerDisplay("{X}, {Y}")]
public readonly struct Coordinate
{
    public int X { get; init; }
    public int Y { get; init; }

    public Coordinate(int x, int y)
    {
        if (x < 0)
            throw new ArgumentOutOfRangeException(nameof(x), "The parameter must not be non-negative.");

        if (y < 0)
            throw new ArgumentOutOfRangeException(nameof(y), "The parameter must not be non-negative.");

        X = x;
        Y = y;
    }

    public static Coordinate operator +(Coordinate lhs, Coordinate rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y);

    public static Coordinate operator -(Coordinate lhs, Coordinate rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y);

    public static bool operator ==(Coordinate? lhs, Coordinate? rhs) => lhs is not null && lhs.Equals(rhs);

    public static bool operator !=(Coordinate? lhs, Coordinate? rhs) => !(lhs == rhs);

    public Coordinate Step(Persistence.IDirection d)
    {
        (int dx, int dy) = d.Diff();
        Coordinate c = new Coordinate(X + dx, Y + dy);
        return c;
    }

    public Dictionary<IDirection, Coordinate> GetAllNeighbors()
    {
        Dictionary<IDirection, Coordinate> coordinates = new();
        foreach (IDirection d in new List<IDirection>([Up.Instance(), Down.Instance(), Left.Instance(), Right.Instance()]))
        {
            try
            {
                coordinates.Add(d, Step(d));
                                        
            }
            catch (Exception e)
            {}
        }
        return coordinates;
    }

    public static bool operator !=(Coordinate lhs, Coordinate rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator==(Coordinate lhs, Coordinate rhs)
    {
        return  lhs.X == rhs.X && lhs.Y == rhs.Y;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Coordinate c)
            return c == this;
        return false;
        
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
    public override string ToString()
    {
        // TYPO HERE: You are returning Y then X, or labels are swapped
        return $"{X}, {Y}"; 
    }

    public static IDirection operator/(Coordinate a, Coordinate b)
    {
        Dictionary<IDirection, Coordinate> n = a.GetAllNeighbors();
        foreach (var d in n.Keys)
        {
            if (n[d] == b) return d;
        }
        Debug.Assert(true, "The specified coordinate does not exist.");
        return Up.Instance();
    }

    public bool IsNeigbor(Coordinate other) => this.GetAllNeighbors().ContainsValue(other);
}
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CSDB_UtopiaModel.Model;

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

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is null) return false;
        if (obj is not Coordinate coord) return false;
        return X == coord.X && Y == coord.Y;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);
}
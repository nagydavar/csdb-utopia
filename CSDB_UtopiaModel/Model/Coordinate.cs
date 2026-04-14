using System.Diagnostics;
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

    public Coordinate Step(Persistence.IDirection d)
    {
        (int dx, int dy) = d.Diff();
        Coordinate c = new Coordinate(X + dx, Y+dy);
        return c;
    }
}
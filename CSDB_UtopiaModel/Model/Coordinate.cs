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

    public Coordinate Step(Persistence.UP _) => new(X, Y - 1);

    public Coordinate Step(Persistence.DOWN _) => new(X, Y + 1);

    public Coordinate Step(Persistence.RIGHT _) => new(X + 1, Y);

    public Coordinate Step(Persistence.LEFT _) => new(X - 1, Y);
}
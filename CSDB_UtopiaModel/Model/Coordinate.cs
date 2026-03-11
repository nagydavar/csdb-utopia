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

    public Coordinate Move(Persistence.Up _) => new(X, Y - 1);

    public Coordinate Move(Persistence.Down _) => new(X, Y + 1);

    public Coordinate Move(Persistence.Right _) => new(X + 1, Y);

    public Coordinate Move(Persistence.Left _) => new(X - 1, Y);
}
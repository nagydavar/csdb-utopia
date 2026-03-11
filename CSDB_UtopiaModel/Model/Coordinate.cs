namespace CSDB_UtopiaModel.Model;

public struct Coordinate
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
}
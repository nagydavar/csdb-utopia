namespace CSDB_UtopiaModel.Model;

public struct Coordinate
{
    public int X { get; init; }
    public int Y { get; init; }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public Coordinate() : this(0, 0) {}
}
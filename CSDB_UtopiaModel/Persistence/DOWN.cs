namespace CSDB_UtopiaModel.Persistence;
class DOWN : VerticalDirection
{
    public (int, int) Diff() => (0, -1);
    public Direction Opposite() => UP.Instance();
    private static DOWN? instance;
    private DOWN() { }
    public static DOWN Instance()
    {
        if (instance is null)
            instance = new DOWN();
        return instance;
    }
};

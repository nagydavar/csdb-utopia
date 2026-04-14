namespace CSDB_UtopiaModel.Persistence;
class UP : VerticalDirection
{
    public (int, int) Diff() => (0, 1);
    public Direction Opposite() => DOWN.Instance();
    private static UP? instance;
    private UP() { }
    public static UP Instance()
    {
        if (instance is null)
            instance = new UP();
        return instance;
    }
};

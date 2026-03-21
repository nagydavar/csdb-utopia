namespace CSDB_UtopiaModel.Persistence;
class LEFT : HorizontalDirection
{
    
    public (int, int) Diff() => (-1, 0);
    public Direction Opposite() => RIGHT.Instance();
    private static LEFT? instance;
    private LEFT() { }
    public static LEFT Instance()
    {
        if (instance is null)
            instance = new LEFT();
        return instance;
    }
};

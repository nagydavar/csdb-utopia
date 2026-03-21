namespace CSDB_UtopiaModel.Persistence;
class RIGHT : HorizontalDirection
{
    public (int, int) Diff() => (1, 0);
    public Direction Opposite() => LEFT.Instance();
    private static RIGHT? instance;
    private RIGHT() { }
    public static RIGHT Instance()
    {
        if (instance is null)
            instance = new RIGHT();
        return instance;
    }
};

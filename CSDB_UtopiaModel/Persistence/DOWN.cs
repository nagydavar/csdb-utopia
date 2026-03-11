namespace CSDB_UtopiaModel.Persistence;
class DOWN : VerticalDirection
{
    private static DOWN? instance;
    private DOWN() { }
    public static DOWN Instance()
    {
        if (instance is null)
            instance = new DOWN();
        return instance;
    }
};

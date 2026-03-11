namespace CSDB_UtopiaModel.Persistence;
class LEFT : HorizontalDirection
{
    private static LEFT? instance;
    private LEFT() { }
    public static LEFT Instance()
    {
        if (instance is null)
            instance = new LEFT();
        return instance;
    }
};

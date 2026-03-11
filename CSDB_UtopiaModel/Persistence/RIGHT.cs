namespace CSDB_UtopiaModel.Persistence;
class RIGHT : HorizontalDirection
{
    private static RIGHT? instance;
    private RIGHT() { }
    public static RIGHT Instance()
    {
        if (instance is null)
            instance = new RIGHT();
        return instance;
    }
};

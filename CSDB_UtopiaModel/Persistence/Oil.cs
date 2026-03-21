namespace CSDB_UtopiaModel.Persistence;
public class Oil : IndustrialResource
{
    private static Oil? instance;
    private Oil() { }
    public static Oil Instance()
    {
        if (instance is null)
            instance = new Oil();
        return instance;
    }
};

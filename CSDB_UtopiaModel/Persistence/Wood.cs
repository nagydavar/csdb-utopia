namespace CSDB_UtopiaModel.Persistence;
public class Wood : IndustrialResource
{
    private static Wood? instance;
    private Wood() { }
    public static Wood Instance()
    {
        if (instance is null)
            instance = new Wood();
        return instance;
    }
};

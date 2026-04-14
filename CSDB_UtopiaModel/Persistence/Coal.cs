namespace CSDB_UtopiaModel.Persistence;
public class Coal : BaseResource, IndustrialResource
{
    private Coal() { }
    private static Coal? instance;
    public static Coal Instance()
    {
        if (instance is null)
            instance = new Coal();
        return instance;
    }
};

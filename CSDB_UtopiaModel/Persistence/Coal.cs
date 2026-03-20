namespace CSDB_UtopiaModel.Persistence;
class Coal : IndustrialResource
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

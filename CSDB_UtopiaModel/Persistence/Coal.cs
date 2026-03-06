namespace CSDB_UtopiaModel.Persistence;
class Coal : IndustrialResource
{
    private Coal() { }
    private Coal? instance;
    public Coal Instance()
    {
        if (instance is null)
            instance = new Coal();
        return instance;
    }
};

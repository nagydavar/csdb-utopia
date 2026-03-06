namespace CSDB_UtopiaModel.Persistence;
class Oil : IndustrialResource
{
    private Oil? instance;
    private Oil() { }
    public Oil Instance()
    {
        if (instance is null)
            instance = new Oil();
        return instance;
    }
};

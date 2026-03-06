namespace CSDB_UtopiaModel.Persistence;
class Wood : IndustrialResource
{
    private Wood? instance;
    private Wood() { }
    public Wood Instance()
    {
        if (instance is null)
            instance = new Wood();
        return instance;
    }
};

namespace CSDB_UtopiaModel.Persistence;
class IronOre : IndustrialResource
{
    private IronOre? instance;
    private IronOre() { }
    public IronOre Instance()
    {
        if (instance is null)
            instance = new IronOre();
        return instance;
    }
};

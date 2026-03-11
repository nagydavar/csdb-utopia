namespace CSDB_UtopiaModel.Persistence;
class IronOre : IndustrialResource
{
    private static IronOre? instance;
    private IronOre() { }
    public static IronOre Instance()
    {
        if (instance is null)
            instance = new IronOre();
        return instance;
    }
};

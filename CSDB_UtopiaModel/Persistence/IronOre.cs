namespace CSDB_UtopiaModel.Persistence;
public class IronOre : IndustrialResource
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

namespace CSDB_UtopiaModel.Persistence;

public class IronOre : BaseResource, IIndustrialResource
{
    private static IronOre? _instance;
    private IronOre() { }
    public static IronOre Instance()
    {
        if (_instance is null)
            _instance = new IronOre();
        return _instance;
    }
}
namespace CSDB_UtopiaModel.Model;

public class IronOre : IndustrialResource
{
    private static IronOre? _instance;
    
    public static IronOre Instance => _instance ??= new();
    
    protected IronOre() {}
}
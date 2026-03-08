namespace CSDB_UtopiaModel.Model;

public class Oil : IndustrialResource
{
    private static Oil? _instance;
    
    public static Oil Instance => _instance ??= new();
    
    protected Oil() {}
}
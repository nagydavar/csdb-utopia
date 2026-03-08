namespace CSDB_UtopiaModel.Model;

public class Coal : IndustrialResource
{
    private static Coal? _instance;
    
    public static Coal Instance => _instance ??= new();
    
    protected Coal() {}
}
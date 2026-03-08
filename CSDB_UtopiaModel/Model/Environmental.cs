namespace CSDB_UtopiaModel.Model;

public class Environmental : Resource
{
    private static Environmental? _instance;
    
    public static Environmental Instance => _instance ??= new();
    
    protected Environmental() {}
}

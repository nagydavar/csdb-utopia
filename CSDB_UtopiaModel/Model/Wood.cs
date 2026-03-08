namespace CSDB_UtopiaModel.Model;

public class Wood
{
    private static Wood?  _instance;
    
    public static Wood? Instance => _instance ??= new();
    
    protected Wood() {}
}
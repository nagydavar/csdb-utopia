namespace CSDB_UtopiaModel.Model;

public class Diamond : Treasure
{
    private static Diamond? _instance;
    
    public static Diamond Instance => _instance ??= new();
    
    protected Diamond() {}
}
namespace CSDB_UtopiaModel.Model;

public class Paper : Goods
{
    private static Paper? _instance;
    
    public static Paper Instance => _instance ??= new();
    
    protected Paper() {}
}
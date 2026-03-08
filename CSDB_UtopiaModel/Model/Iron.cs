namespace CSDB_UtopiaModel.Model;

public class Iron : Goods
{
    private static Iron? _instance;
    
    public static Iron Instance => _instance ??= new();
    
    protected Iron() {}
}
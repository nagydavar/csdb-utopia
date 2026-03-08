namespace CSDB_UtopiaModel.Model;

public class Gasoline : Goods
{
    private static Gasoline? _instance;
    
    public static Gasoline Instance => _instance ??= new();
    
    protected Gasoline() {}
}
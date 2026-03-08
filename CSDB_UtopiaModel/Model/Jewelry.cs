namespace CSDB_UtopiaModel.Model;

public class Jewelry : Goods
{
    private static Jewelry? _instance;
    
    public static Jewelry Instance => _instance ??= new();
    
    protected Jewelry() {}
}
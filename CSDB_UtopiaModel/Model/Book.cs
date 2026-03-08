namespace CSDB_UtopiaModel.Model;

public class Book : Goods
{
    private static Book? _instance;
    
    public static Book Instance => _instance ??= new();
    
    protected Book() {}
}
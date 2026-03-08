namespace CSDB_UtopiaModel.Model;

public class Plank : Goods
{
    private static Plank? _instance;
    
    public static Plank Instance => _instance ??= new();
    
    protected Plank() {}
}
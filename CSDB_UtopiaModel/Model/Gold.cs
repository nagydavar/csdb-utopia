using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class Gold : Treasure
{
    private static Gold? _instance;
    
    public static Gold Instance => _instance ??= new();
    
    protected Gold() {}
}
namespace CSDB_UtopiaModel.Persistence;

public class Environmental : BaseResource
{
    private static Environmental? _instance;
    private Environmental() { }
    public static Environmental Instance()
    {
        if (_instance is null)
            _instance = new Environmental();
        return _instance;
    }
}
namespace CSDB_UtopiaModel.Persistence;

public class Oil : BaseResource, IIndustrialResource
{
    private static Oil? _instance;
    private Oil() { }
    public static Oil Instance()
    {
        if (_instance is null)
            _instance = new Oil();
        return _instance;
    }
}
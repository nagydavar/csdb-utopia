namespace CSDB_UtopiaModel.Persistence;

public class Coal : BaseResource, IIndustrialResource
{
    private Coal() { }
    private static Coal? _instance;
    public static Coal Instance()
    {
        if (_instance is null)
            _instance = new Coal();
        return _instance;
    }
}
namespace CSDB_UtopiaModel.Persistence;

public class Wood : BaseResource, IIndustrialResource
{
    private static Wood? _instance;
    private Wood() { }
    public static Wood Instance()
    {
        if (_instance is null)
            _instance = new Wood();
        return _instance;
    }
}
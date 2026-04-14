namespace CSDB_UtopiaModel.Persistence;

public class HumanResource : BaseResource, IResource
{
    private static HumanResource? _instance;
    private HumanResource() { }
    public static HumanResource Instance()
    {
        if (_instance is null)
            _instance = new HumanResource();
        return _instance;
    }
}
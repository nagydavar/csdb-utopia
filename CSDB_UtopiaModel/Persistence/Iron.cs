namespace CSDB_UtopiaModel.Persistence;

public class Iron : BaseResource, IGoods
{
    private Iron() { }
    private static Iron? _instance;
    public static Iron Instance()
    {
        if (_instance is null)
            _instance = new Iron();
        return _instance;
    }
}
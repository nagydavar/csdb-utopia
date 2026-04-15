namespace CSDB_UtopiaModel.Persistence;

public class Gasoline : BaseResource, IGoods
{
    private Gasoline() { }
    private static Gasoline? _instance;
    public static Gasoline Instance()
    {
        if (_instance is null)
            _instance = new Gasoline();
        return _instance;
    }
}
namespace CSDB_UtopiaModel.Persistence;

public class Jewelry : BaseResource, IGoods
{
    private static Jewelry? _instance;
    private Jewelry() { }
    public static Jewelry Instance()
    {
        if (_instance is null)
            _instance = new Jewelry();
        return _instance;
    }
}
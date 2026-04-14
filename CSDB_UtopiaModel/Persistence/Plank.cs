namespace CSDB_UtopiaModel.Persistence;

public class Plank : BaseResource, IGoods
{
    private static Plank? _instance;
    private Plank() { }
    public static Plank Instance()
    {
        if (_instance is null)
            _instance = new Plank();
        return _instance;
    }
}
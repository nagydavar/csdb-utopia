namespace CSDB_UtopiaModel.Persistence;

public class Paper : BaseResource, IGoods
{
    private static Paper? _instance;
    private Paper() { }
    public static Paper Instance()
    {
        if (_instance is null)
            _instance = new Paper();
        return _instance;
    }
}
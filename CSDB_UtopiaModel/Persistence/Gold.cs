namespace CSDB_UtopiaModel.Persistence;

public class Gold : Treasure
{
    private static Gold? _instance;
    private Gold() { }
    public static Gold Instance()
    {
        if (_instance is null)
            _instance = new Gold();
        return _instance;
    }
}
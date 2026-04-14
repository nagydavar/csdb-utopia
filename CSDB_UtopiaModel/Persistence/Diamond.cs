namespace CSDB_UtopiaModel.Persistence;

public class Diamond : Treasure
{
    private static Diamond? _instance;
    private Diamond() { }
    public static Diamond Instance()
    {
        if (_instance is null)
            _instance = new Diamond();
        return _instance;
    }
}
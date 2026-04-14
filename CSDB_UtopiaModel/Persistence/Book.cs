namespace CSDB_UtopiaModel.Persistence;

public class Book : BaseResource, IGoods
{
    private Book() { }
    private static Book? _instance;
    public static Book Instance()
    {
        if (_instance is null)
            _instance = new Book();
        return _instance;
    }
}
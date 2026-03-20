namespace CSDB_UtopiaModel.Persistence;
public class Book : Goods
{

    private Book() { }
    private static Book? instance;
    public static Book Instance()
    {
        if (instance is null)
            instance = new Book();
        return instance;
    }
};

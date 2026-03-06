namespace CSDB_UtopiaModel.Persistence;
class Book : Goods
{

    private Book() { }
    private Book? instance;
    public Book Instance()
    {
        if (instance is null)
            instance = new Book();
        return instance;
    }
};

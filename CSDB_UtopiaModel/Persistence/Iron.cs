namespace CSDB_UtopiaModel.Persistence;
class Iron : Goods
{
    private Iron() { }
    private static Iron? instance;
    public static Iron Instance()
    {
        if (instance is null)
            instance = new Iron();
        return instance;
    }
};

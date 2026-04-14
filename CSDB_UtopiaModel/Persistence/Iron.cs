namespace CSDB_UtopiaModel.Persistence;
public class Iron : BaseResource, Goods
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

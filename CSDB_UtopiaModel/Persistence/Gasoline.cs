namespace CSDB_UtopiaModel.Persistence;
public class Gasoline : BaseResource, Goods
{
    private Gasoline() { }
    private static Gasoline? instance;
    public static Gasoline Instance()
    {
        if (instance is null)
            instance = new Gasoline();
        return instance;
    }
};

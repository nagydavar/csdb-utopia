namespace CSDB_UtopiaModel.Persistence;
public class Paper : BaseResource, Goods
{
    private static Paper? instance;
    private Paper() { }
    public static Paper Instance()
    {
        if (instance is null)
            instance = new Paper();
        return instance;
    }
};

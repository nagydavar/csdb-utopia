namespace CSDB_UtopiaModel.Persistence;
public class Plank : BaseResource, Goods
{
    private static Plank? instance;
    private Plank() { }
    public static Plank Instance()
    {
        if (instance is null)
            instance = new Plank();
        return instance;
    }
};

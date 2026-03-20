namespace CSDB_UtopiaModel.Persistence;
public class Enviromental : Resource
{
    private static Enviromental? instance;
    private Enviromental() { }
    public static Enviromental Instance()
    {
        if (instance is null)
            instance = new Enviromental();
        return instance;
    }
};

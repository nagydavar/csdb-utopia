namespace CSDB_UtopiaModel.Persistence;
public class Enviromental : Resource
{
    private Enviromental? instance;
    private Enviromental() { }
    public Enviromental Instance()
    {
        if (instance is null)
            instance = new Enviromental();
        return instance;
    }
};

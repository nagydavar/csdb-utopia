using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Enviromental : Resource
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

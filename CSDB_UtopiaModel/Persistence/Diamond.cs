using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;
class Diamond : Treasure
{
    private Diamond? instance;
    private Diamond() { }
    public Diamond Instance()
    {
        if (instance is null)
            instance = new Diamond();
        return instance;
    }
};

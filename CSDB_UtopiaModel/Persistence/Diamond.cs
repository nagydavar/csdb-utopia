using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;
class Diamond : Treasure
{
    private static Diamond? instance;
    private Diamond() { }
    public static Diamond Instance()
    {
        if (instance is null)
            instance = new Diamond();
        return instance;
    }
};

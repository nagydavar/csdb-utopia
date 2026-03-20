using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;
class Gold : Treasure
{
    private static Gold? instance;
    private Gold() { }
    public static Gold Instance()
    {
        if (instance is null)
            instance = new Gold();
        return instance;
    }
};

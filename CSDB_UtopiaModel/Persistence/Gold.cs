using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;
class Gold : Treasure
{
    private Gold? instance;
    private Gold() { }
    public Gold Instance()
    {
        if (instance is null)
            instance = new Gold();
        return instance;
    }
};

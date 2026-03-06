namespace CSDB_UtopiaModel.Model;
class UP : VerticalDirection
{
    private UP? instance;
    private UP() { }
    public UP Instance()
    {
        if (instance is null)
            instance = new UP();
        return instance;
    }
};

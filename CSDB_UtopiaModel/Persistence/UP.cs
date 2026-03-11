namespace CSDB_UtopiaModel.Persistence;
class UP : VerticalDirection
{
    private static UP? instance;
    private UP() { }
    public static UP Instance()
    {
        if (instance is null)
            instance = new UP();
        return instance;
    }
};

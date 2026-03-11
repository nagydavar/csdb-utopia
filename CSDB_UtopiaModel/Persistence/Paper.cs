namespace CSDB_UtopiaModel.Persistence;
class Paper : Goods
{
    private static Paper? instance;
    private Paper() { }
    public static Paper Instance()
    {
        if (instance is null)
            instance = new Paper();
        return instance;
    }
};

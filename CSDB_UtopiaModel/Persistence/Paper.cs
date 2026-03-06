namespace CSDB_UtopiaModel.Persistence;
class Paper : Goods
{
    private Paper? instance;
    private Paper() { }
    public Paper Instance()
    {
        if (instance is null)
            instance = new Paper();
        return instance;
    }
};

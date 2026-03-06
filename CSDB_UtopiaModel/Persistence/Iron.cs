namespace CSDB_UtopiaModel.Persistence;
class Iron : Goods
{
    private Iron() { }
    private Iron? instance;
    public Iron Instance()
    {
        if (instance is null)
            instance = new Iron();
        return instance;
    }
};

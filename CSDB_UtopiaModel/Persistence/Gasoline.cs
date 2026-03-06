namespace CSDB_UtopiaModel.Persistence;
class Gasoline : Goods
{
    private Gasoline() { }
    private Gasoline? instance;
    public Gasoline Instance()
    {
        if (instance is null)
            instance = new Gasoline();
        return instance;
    }
};

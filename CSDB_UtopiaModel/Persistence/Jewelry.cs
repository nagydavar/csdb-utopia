namespace CSDB_UtopiaModel.Persistence;
class Jewelry : Goods
{
    private Jewelry? instance;
    private Jewelry() { }
    public Jewelry Instance()
    {
        if (instance is null)
            instance = new Jewelry();
        return instance;
    }
};

namespace CSDB_UtopiaModel.Persistence;
class Plank : Goods
{
    private Plank? instance;
    private Plank() { }
    public Plank Instance()
    {
        if (instance is null)
            instance = new Plank();
        return instance;
    }
};

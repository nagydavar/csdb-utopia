namespace CSDB_UtopiaModel.Model;
class LEFT : HorizontalDirection
{
    private LEFT? instance;
    private LEFT() { }
    public LEFT Instance()
    {
        if (instance is null)
            instance = new LEFT();
        return instance;
    }
};

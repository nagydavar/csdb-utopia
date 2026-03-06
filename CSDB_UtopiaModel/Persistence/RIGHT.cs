namespace CSDB_UtopiaModel.Persistence;
class RIGHT : HorizontalDirection
{
    private RIGHT? instance;
    private RIGHT() { }
    public RIGHT Instance()
    {
        if (instance is null)
            instance = new RIGHT();
        return instance;
    }
};

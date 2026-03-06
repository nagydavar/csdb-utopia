namespace CSDB_UtopiaModel.Model;
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

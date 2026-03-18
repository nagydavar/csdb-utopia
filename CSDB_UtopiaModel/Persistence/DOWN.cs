namespace CSDB_UtopiaModel.Persistence;
public class DOWN : VerticalDirection
{
    private DOWN? instance;
    private DOWN() { }
    public DOWN Instance()
    {
        if (instance is null)
            instance = new DOWN();
        return instance;
    }
};

namespace CSDB_UtopiaModel.Persistence;
public class HumanResource : Resource
{
    private static HumanResource? instance;
    private HumanResource() { }
    public static HumanResource Instance()
    {
        if (instance is null)
            instance = new HumanResource();
        return instance;
    }
};

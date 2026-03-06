namespace CSDB_UtopiaModel.Persistence;
class HumanResource : Resource
{
    private HumanResource? instance;
    private HumanResource() { }
    public HumanResource Instance()
    {
        if (instance is null)
            instance = new HumanResource();
        return instance;
    }
};

namespace CSDB_UtopiaModel.Model;

public class HumanResource : Resource
{
    private static HumanResource? _instance;
    
    public static HumanResource Instance => _instance ??= new ();
    
    protected HumanResource() {}
}
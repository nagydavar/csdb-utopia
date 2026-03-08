namespace CSDB_UtopiaModel.Model;

class Environmental : Resource
{
    private Environmental? _instance;
    
    public Environmental Instance => _instance ??= new();
    
    protected Environmental() {}
};

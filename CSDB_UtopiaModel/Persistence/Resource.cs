namespace CSDB_UtopiaModel.Persistence;
public interface Resource
{
    string IconPath { get; }
}

public abstract class BaseResource : Resource
{
    // Ez csak egyszer fut le, és a konkrét gyerek típusnevét használja
    public string IconPath => $"Icons/{GetType().Name}.PNG";
}

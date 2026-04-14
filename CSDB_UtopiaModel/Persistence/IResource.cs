namespace CSDB_UtopiaModel.Persistence;

public interface IResource
{
    string IconPath { get; }
}

public abstract class BaseResource : IResource
{
    // Ez csak egyszer fut le, �s a konkr�t gyerek t�pusnev�t haszn�lja
    public string IconPath => $"Icons/{GetType().Name}.PNG";
}
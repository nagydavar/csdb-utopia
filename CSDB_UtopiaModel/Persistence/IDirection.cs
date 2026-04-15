namespace CSDB_UtopiaModel.Persistence;

public interface IDirection
{
    public abstract (int, int) Diff();
    public abstract IDirection Opposite();
}
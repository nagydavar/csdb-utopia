namespace CSDB_UtopiaModel.Persistence;

public interface IDirection
{
    (int, int) Diff();

    IDirection Opposite();

    IDirection FromPerspectiveOf(IDirection pers);
}
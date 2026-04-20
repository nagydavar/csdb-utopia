namespace CSDB_UtopiaModel.Persistence;

public class Up : VerticalDirection
{
    public (int, int) Diff() => (0, 1);
    public IDirection Opposite() => Down.Instance();
    private static Up? _instance;
    private Up() { }
    public static Up Instance()
    {
        if (_instance is null)
            _instance = new Up();
        return _instance;
    }

    public IDirection FromPerspectiveOf(IDirection pers) => pers switch
    {
        Up => Down.Instance(),
        Down => this,
        _ => pers,
    };
}
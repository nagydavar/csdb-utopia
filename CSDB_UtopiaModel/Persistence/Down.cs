namespace CSDB_UtopiaModel.Persistence;

public class Down : VerticalDirection
{
    public (int, int) Diff() => (0, -1);
    public IDirection Opposite() => Up.Instance();
    private static Down? _instance;
    private Down() { }
    public static Down Instance()
    {
        if (_instance is null)
            _instance = new Down();
        return _instance;
    }

    public IDirection FromPerspectiveOf(IDirection pers) => pers switch
    {
        Up => Up.Instance(),
        Down => this,
        _ => pers.Opposite()
    };
}
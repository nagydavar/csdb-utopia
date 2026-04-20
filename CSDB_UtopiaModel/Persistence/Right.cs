namespace CSDB_UtopiaModel.Persistence;

public class Right : IHorizontalDirection
{
    public (int, int) Diff() => (1, 0);
    public IDirection Opposite() => Left.Instance();
    private static Right? _instance;
    private Right() { }
    public static Right Instance()
    {
        if (_instance is null)
            _instance = new Right();
        return _instance;
    }

    public IDirection FromPerspectiveOf(IDirection pers) => pers switch
    {
        Right => Up.Instance(),
        Up => Left.Instance(),
        Left => Up.Instance(),
        _ => this
    };
}
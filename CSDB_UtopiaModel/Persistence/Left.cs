namespace CSDB_UtopiaModel.Persistence;

public class Left : IHorizontalDirection
{
    public (int, int) Diff() => (-1, 0);
    public IDirection Opposite() => Right.Instance();
    private static Left? _instance;
    private Left() { }
    public static Left Instance()
    {
        if (_instance is null)
            _instance = new Left();
        return _instance;
    }

    public IDirection FromPerspectiveOf(IDirection pers) => pers switch
    {
        Up => Right.Instance(),
        Right => Up.Instance(),
        Left => Down.Instance(),
        _ => this
    };
}
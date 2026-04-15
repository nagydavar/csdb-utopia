using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class DirectionEventArgs: EventArgs
{
    public IDirection Direction { get; init; }

    public DirectionEventArgs(IDirection direction) => Direction = direction;
}
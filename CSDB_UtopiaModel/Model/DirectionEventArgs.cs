using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class DirectionEventArgs: EventArgs
{
    public Direction Direction { get; init; }

    public DirectionEventArgs(Direction direction) => Direction = direction;
}
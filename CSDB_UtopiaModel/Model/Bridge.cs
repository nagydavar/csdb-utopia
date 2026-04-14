using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Bridge : Road
{
    protected Bridge(Field f, int maxSpeed, Direction d) : base(f, maxSpeed, d)
    {
    }
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class WoodenBridge : Bridge
{
    public WoodenBridge(Field f, Direction d) : base(f, 10, d)
    {
        //TODO value of maxSpeed
    }
}
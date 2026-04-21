using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class WoodenBridge : Bridge
{
    public override int MaxLength => 5;

    public WoodenBridge(Field f, IDirection d) : base(f, 10, d)
    {
        //TODO value of maxSpeed
    }
}
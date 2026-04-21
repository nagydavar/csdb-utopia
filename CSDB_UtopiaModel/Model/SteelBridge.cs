using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class SteelBridge : Bridge
{
    public override int MaxLength => 25;

    public SteelBridge(Field f, IDirection d) : base(f, 30, d)
    {
        //TODO value of maxSpeed
    }
}
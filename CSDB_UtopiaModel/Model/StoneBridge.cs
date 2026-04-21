using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class StoneBridge : Bridge
{
    public override int MaxLength => 10;

    public override int placementCost => 150;

    public StoneBridge(Field f, IDirection d) : base(f, 20, d)
    {
        //TODO value of maxSpeed
    }
}
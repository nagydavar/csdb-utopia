using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Bridge : Road
{
    public abstract int MaxLength { get; }

    protected Bridge(Field f, int maxSpeed, IDirection d) : base(f, maxSpeed, d)
    {
    }

    public Bridge Clone(Field f) => this switch
    {
        SteelBridge => new SteelBridge(f, Direction)
        {
            MaxSpeed = MaxSpeed,
            IsCurved = IsCurved,
            Quadrant = Quadrant,
        },
        StoneBridge => new StoneBridge(f, Direction)
        {
            MaxSpeed = MaxSpeed,
            IsCurved = IsCurved,
            Quadrant = Quadrant,
        },
        WoodenBridge => new WoodenBridge(f, Direction)
        {
            MaxSpeed = MaxSpeed,
            IsCurved = IsCurved,
            Quadrant = Quadrant,
        },
        _ => throw new NotImplementedException()
    };
}
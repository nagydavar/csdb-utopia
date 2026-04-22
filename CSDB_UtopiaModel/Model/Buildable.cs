using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Buildable : Buyable
{
    public virtual int placementCost { get; }
    public (int Width, int Height) area { get; protected set; }
    public Field Owner { get; protected set; }

    public Buildable(Field f)
    {
        Owner = f;
    }
}
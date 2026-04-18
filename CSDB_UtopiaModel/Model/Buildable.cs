using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public abstract class Buildable : Buyable
{
    public virtual int placementCost { get; }
    public (int Width, int Height) area { get; protected set; }
    protected readonly Field owner;
    protected Coordinate field;
    public Buildable(Field f)
    {
        owner = f;
        //owner.Place(this);
    }
}

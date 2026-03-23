using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public abstract class Buildable : Buyable
{
    public virtual int placementCost { get; }
    protected (int, int) area;
    protected Field owner;
    protected Coordinate field;
    public Buildable(Field f)
    {
        owner = f;
        //owner.Place(this);
    }
}

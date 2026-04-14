using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Decoration : Building
{
    public int giveMood { get; set; }
    public (IResource resource, int cost) costResource;
    public Decoration(Field f) : base(f) { }
};

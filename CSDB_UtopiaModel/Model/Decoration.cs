using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Decoration : Building
{
    protected int giveMood;
    protected (Resource resource, int cost) costResource;
    public Decoration(Field f) : base(f) { }
};

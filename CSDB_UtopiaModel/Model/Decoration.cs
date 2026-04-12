using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Decoration : Building
{
    public int giveMood { get; set; }
    public (Resource resource, int cost) costResource;
    public Decoration(Field f) : base(f) { }
};

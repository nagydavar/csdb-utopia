using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Colosseum : Decoration
{
    
    public Colosseum(Field f) : base(f)
    {
        giveMood = 300;
        costResource = (Iron.Instance(), 700);
        area = (3, 3);
    }

    public override int placementCost => 4000;
};

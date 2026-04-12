using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Stadium: Decoration
{
    public Stadium(Field f) : base(f)
    {
        giveMood = 200;
        costResource = (Iron.Instance(), 500);
        area = (3, 4);
    }

    public override int placementCost => 4200;
};

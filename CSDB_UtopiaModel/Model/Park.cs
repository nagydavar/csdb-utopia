using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Park: Decoration
{
    public Park(Field f) : base(f)
    {
        giveMood = 20;
        costResource = (Wood.Instance(), 200);
        area = (2, 2);
    }

    public override int placementCost => 1500;
};

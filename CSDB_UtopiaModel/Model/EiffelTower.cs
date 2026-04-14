using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class EiffelTower : Decoration
{
    public EiffelTower(Field f) : base(f)
    {
        giveMood = 350;
        costResource = (Iron.Instance(), 1000); // Vasba ker�l
        area = (2, 2);
    }

    public override int placementCost => 5000;
};

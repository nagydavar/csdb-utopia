using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Funfair: Decoration
{
    public Funfair(Field f) : base(f)
    {
        giveMood = 40;
        costResource = (Iron.Instance(), 100); // Vasba ker�l
        area = (2, 3);
    }

    public override int placementCost => 1300;
};

using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Park: Decoration
{
    public Park(Field f) : base(f)
    {
        giveMood = 100;
        costResource = (Iron.Instance(), 700);
        area = (3, 3);
    }
};

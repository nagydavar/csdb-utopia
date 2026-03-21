using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Stadium: Decoration
{
    public Stadium(Field f) : base(f)
    {
        giveMood = 50;
        costResource = (Iron.Instance(), 500);
        area = (4, 4);
    }
};

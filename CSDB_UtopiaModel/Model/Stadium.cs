using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Stadium: Decoration
{
    public Stadium(int x, int y) : base(x, y)
    {
        giveMood = 50;
        costResource = (Iron.Instance(), 500);
        area = (4, 4);
    }
};

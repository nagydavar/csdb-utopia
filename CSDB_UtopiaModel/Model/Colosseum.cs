using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Colosseum : Decoration
{
    public Colosseum(int x, int y) : base(x, y)
    {
        giveMood = 200;
        costResource = (Iron.Instance(), 700);
        area = (3, 3);
    }
};

using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Funfair: Decoration
{
    public Funfair(int x, int y) : base(x, y)
    {
        giveMood = 10;
        costResource = (Iron.Instance(), 100); // Vasba kerül
        area = (2, 2);
    }
};

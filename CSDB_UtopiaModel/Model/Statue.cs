using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Statue: Decoration
{
    public Statue(int x, int y) : base(x, y)
    {
        giveMood = 50;
        costResource = (Gold.Instance, 500);
        area = (1, 1);
    }
};

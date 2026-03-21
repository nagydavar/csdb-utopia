using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class EiffelTower : Decoration
{
    public EiffelTower(int x, int y) : base(x, y)
    {
        giveMood = 100;
        costResource = (Iron.Instance(), 1000); // Vasba kerül
        area = (2, 2);
    }
};

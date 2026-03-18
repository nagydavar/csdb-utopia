using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Library: Decoration
{
    public Library(int x, int y) : base(x, y)
    {
        // Példa értékek
        giveMood = 10;
        costResource = (Paper.Instance(), 50); // Papírba kerül
        area = (1,1);
    }
};

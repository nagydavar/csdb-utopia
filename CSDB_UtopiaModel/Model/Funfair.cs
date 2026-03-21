using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Funfair: Decoration
{
    public Funfair(Field f) : base(f)
    {
        giveMood = 10;
        costResource = (Iron.Instance(), 100); // Vasba ker�l
        area = (2, 2);
    }
};

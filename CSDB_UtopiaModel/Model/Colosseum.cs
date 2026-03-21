using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Colosseum : Decoration
{
    public Colosseum(Field f) : base(f)
    {
        giveMood = 200;
        costResource = (Iron.Instance(), 700);
        area = (3, 3);
    }
};

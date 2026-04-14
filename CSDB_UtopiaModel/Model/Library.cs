using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Library: Decoration
{
    public Library(Field f) : base(f) 
    {
        // P�lda �rt�kek
        giveMood = 20;
        costResource = (Paper.Instance(), 50); // Pap�rba ker�l
        area = (1,1);
    }

    public override int placementCost => 1000;
};

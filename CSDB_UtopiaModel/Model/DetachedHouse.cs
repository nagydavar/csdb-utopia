using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class DetachedHouse : Building, IResidentialBuilding
{
    public DetachedHouse(Field f) : base(f) { }

    // 1. Publikusnak kell lennie az interf�sz miatt
    // 2. Property-k�nt kell megval�s�tani (get)
    // 3. Csak akkor override, ha a Building-ben is benne van virtual-k�nt
    public int givePeople => 5;

    public int AffectMood => -2;

    // A Buildable-b�l �r�k�lt k�telez� elem
    public override int placementCost => 500;
};

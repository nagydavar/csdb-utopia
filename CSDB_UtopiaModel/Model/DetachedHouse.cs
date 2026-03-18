namespace CSDB_UtopiaModel.Model;
public class DetachedHouse : Building, IResidentialBuilding
{
    public DetachedHouse(int x, int y) : base(x, y) { }

    // 1. Publikusnak kell lennie az interfész miatt
    // 2. Property-ként kell megvalósítani (get)
    // 3. Csak akkor override, ha a Building-ben is benne van virtual-ként
    public int givePeople => 5;

    public int AffectMood => -2;

    // A Buildable-bõl örökölt kötelezõ elem
    public override int placementCost => 500;
};

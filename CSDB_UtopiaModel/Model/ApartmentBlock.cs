namespace CSDB_UtopiaModel.Model;
class ApartmentBlock : Building, IResidentialBuilding
{
    public ApartmentBlock(int x, int y) : base(x, y) { }

    public int givePeople => 20;

    public int AffectMood => -10;

    //valami értékek dummy
    public override int placementCost => 2000;
    };

using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class ApartmentBlock : Building, IResidentialBuilding
{
    public ApartmentBlock(Field f) : base(f) { }

    public int givePeople => 20;

    public int AffectMood => -10;

    //valami �rt�kek dummy
    public override int placementCost => 2000;
    };

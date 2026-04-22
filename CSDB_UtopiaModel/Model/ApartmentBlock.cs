using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class ApartmentBlock : Building, IResidentialBuilding
{
    public ApartmentBlock(Field f) : base(f) {
        area = (1, 1);
    }

    public int givePeople => 20;

    public int AffectMood => -10;
    public Stop ConnectsTo { get; set; }

    //valami �rt�kek dummy
    public override int placementCost => 2000;
    };

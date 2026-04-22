using System.Runtime.CompilerServices;

namespace CSDB_UtopiaModel.Model;
public interface IResidentialBuilding
{
    int givePeople { get; }
    int AffectMood { get; }
    
    public Stop ConnectsTo { get; set; }

};

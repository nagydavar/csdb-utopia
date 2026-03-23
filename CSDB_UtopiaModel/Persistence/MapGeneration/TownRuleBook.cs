using System.Diagnostics;

namespace CSDB_UtopiaModel.Persistence.MapGeneration;

class TownRuleBook : RuleBook
{


    public TownRuleBook():base()
    {
        proportions[FieldTypes.Land] = 0;
        proportions[FieldTypes.Forest] = 0;
        proportions[FieldTypes.Water] = 0;
        proportions[FieldTypes.Mountain] = 0;
    }
}
using System.Collections.Frozen;

namespace CSDB_UtopiaModel.Persistence.MapGeneration
{
    internal class Rules
    {

        private Dictionary<Direction, HashSet<FieldTypes>> rules;

        FrozenSet<FieldTypes> GetRule(Direction d)
        {
            return rules[d].ToFrozenSet<FieldTypes>();
        }
    }
}

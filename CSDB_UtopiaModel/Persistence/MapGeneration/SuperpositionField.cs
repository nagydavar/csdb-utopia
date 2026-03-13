using System.Collections.Frozen;

namespace CSDB_UtopiaModel.Persistence.MapGeneration
{
    internal class SuperpositionField
    {
        public bool Collapsed { get; private set; }
        private HashSet<FieldTypes> positions;
        private Rules rules;
        public int Entropy { get => positions.Count; }
        public SuperpositionField(Rules r)
        {
            rules = r;
            positions = new(Enum.GetValues<FieldTypes>());
        }
        Field ToField()
        {
            if (!Collapsed) throw new InvalidOperationException("Not collapsed yet!");
            //TODO:
            return new Field();
        }
        void Restrict(FrozenSet<FieldTypes> rule)
        {
            positions.IntersectWith(rule);
        }
    }
}

using System.Text;
using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence.MapGeneration
{
    internal class SuperpositionField
    {
        public bool Collapsed { get; private set; }
        private RuleBook ruleBook;
        private HashSet<FieldTypes> positions;
        public Coordinate Coordinate { get; private set; }
        private RuleForCell ruleForCell;
        public RuleForCell RuleForCell {get => ruleForCell.Copy();}
        public int Entropy { get => positions.Count; }
        public SuperpositionField(RuleForCell r, Coordinate coord, RuleBook ruleBook)
        {
            this.ruleBook = ruleBook;
            Coordinate = coord;
            ruleForCell = r;
            positions = new(Enum.GetValues<FieldTypes>());
        }
        /*Field ToField()
        {
            if (!Collapsed) throw new InvalidOperationException("Not collapsed yet!");
            //TODO:
            return new Field();
        }*/
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var p in positions) 
            {
               sb.Append(p);
               sb.Append(',');
            }

            return sb.ToString();
        }

        public bool Restrict(Rule rule)
        {
            if (Collapsed) return false;
            int c = Entropy;
            positions.IntersectWith(rule);
            if (positions.Count == 0) throw new MapGenerationConflictException();
            ruleForCell = new RuleForCell();
            foreach (FieldTypes f in positions)
            {
                ruleForCell.UnionWith(ruleBook.Get(f));
            }

            return c != Entropy;
        }

        public RuleForCell Collapse(Random r)
        {
            Collapsed = true;
            FieldTypes f = positions.ToList()[r.Next(Entropy)];
            positions = new HashSet<FieldTypes>([f]);
            ruleForCell = ruleBook.Get(f);
            return ruleForCell.Copy();
        }
        
    }
}

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
        public Field ToField()
        {
            if (!Collapsed) throw new InvalidOperationException("Not collapsed yet!");
            //TODO:
            FieldTypes ft = positions.ToList()[0];
            Field field = new Land(Coordinate, 0, false);
            Motorway m;
            switch (ft)
            {

                case FieldTypes.LAND:
                    //field = new Land(Coordinate, 0, false);
                    break;
                case FieldTypes.FOREST:
                    field = new Land(Coordinate, 3, true);
                    break;
                case FieldTypes.WATER:
                    field = new Water(Coordinate);
                    break;
                case FieldTypes.ROAD_HOR:
                    m = new Motorway(field, 20, UP.Instance());
                    break;
                case FieldTypes.ROAD_VER:
                    m = new Motorway(field, 20, LEFT.Instance());
                    break;
                case FieldTypes.FOUR_INTER:
                    m = new Motorway(field, 20, UP.Instance());
                    Intersection i4 = new FourWayIntersection(field);
                    m.AddIntersection(i4);
                    break;

                case FieldTypes.THREE_INTER_HOR_UP:
                    m = new Motorway(field, 20, UP.Instance());
                    Intersection i30 = new ThreeWayIntersection(field, UP.Instance());
                    m.AddIntersection(i30);
                    break;
                
                case FieldTypes.THREE_INTER_HOR_DOWN:
                    m = new Motorway(field, 20, DOWN.Instance());
                    Intersection i31 = new ThreeWayIntersection(field, DOWN.Instance());
                    m.AddIntersection(i31);
                    break;

                case FieldTypes.THREE_INTER_VER_LEFT:
                    m = new Motorway(field, 20, LEFT.Instance());
                    Intersection i32 = new ThreeWayIntersection(field, LEFT.Instance());
                    m.AddIntersection(i32);
                    break;
                
                case FieldTypes.THREE_INTER_VER_RIGHT:
                    m = new Motorway(field, 20, RIGHT.Instance());
                    Intersection i33 = new ThreeWayIntersection(field, RIGHT.Instance());
                    m.AddIntersection(i33);
                    break;
            }
            return field;
        }
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

using System.Diagnostics;
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
            positions = new(ruleBook.possibleFields);
        }

        public FieldTypes ToFieldType()
        {
            if (!Collapsed) throw new InvalidOperationException("Not collapsed yet!"); ;
            return positions.Last();
        }
        
        public Field ToField(Random r)
        {
            if (!Collapsed) throw new InvalidOperationException("Not collapsed yet!");
            //TODO:
            FieldTypes ft = positions.ToList()[0];
            Field field = new Land(Coordinate, 0, false);
            Motorway m;
            switch (ft)
            {

                case FieldTypes.Land:
                    //field = new Land(Coordinate, 0, false);
                    break;
                case FieldTypes.Forest:
                    field = new Land(Coordinate, r.Next(5), true);
                    break;
                case FieldTypes.Water:
                    field = new Water(Coordinate);
                    break;
                case FieldTypes.Mountain:
                    field = new Mountain(Coordinate);
                    break;
                case FieldTypes.RoadHor:
                    m = new Motorway(field, 20, UP.Instance());
                    break;
                case FieldTypes.RoadVer:
                    m = new Motorway(field, 20, LEFT.Instance());
                    break;
                case FieldTypes.FourInter:
                    m = new Motorway(field, 20, UP.Instance());
                    Intersection i4 = new FourWayIntersection(field);
                    m.AddIntersection(i4);
                    break;

                case FieldTypes.ThreeInterHorUp:
                    m = new Motorway(field, 20, UP.Instance());
                    Intersection i30 = new ThreeWayIntersection(field, UP.Instance());
                    m.AddIntersection(i30);
                    break;
                
                case FieldTypes.ThreeInterHorDown:
                    m = new Motorway(field, 20, DOWN.Instance());
                    Intersection i31 = new ThreeWayIntersection(field, DOWN.Instance());
                    m.AddIntersection(i31);
                    break;

                case FieldTypes.ThreeInterVerLeft:
                    m = new Motorway(field, 20, LEFT.Instance());
                    Intersection i32 = new ThreeWayIntersection(field, LEFT.Instance());
                    m.AddIntersection(i32);
                    break;
                
                case FieldTypes.ThreeInterVerRight:
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
            //Console.WriteLine("Restrict " +"(" + Coordinate.X + ", " + Coordinate.Y + ")" );
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
            FieldTypes f = ruleBook.ProportionalRandom(r, positions.ToList());
            //FieldTypes f = positions.ToList()[r.Next(Entropy)];
            positions = new HashSet<FieldTypes>([f]);
            ruleForCell = ruleBook.Get(f);
            return ruleForCell.Copy();
        }

        
        
    }
}

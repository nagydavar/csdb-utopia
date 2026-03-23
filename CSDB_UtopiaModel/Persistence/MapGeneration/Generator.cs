using System.Text;
using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence.MapGeneration

{
    class Generator
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        private RuleBook ruleBook;
        private List<SuperpositionField> fields;
        private Stack<SuperpositionField> fieldStack;
        private Random random;
        private int triedTimes = 0;

        private static int maxTriedTimes = 20;
        //private int collapsed = 0;


        public Generator(int width, int height, RuleBook rb)
        {
            random = new Random();
            Height = height;
            Width = width;
            ruleBook = rb;
            if (height <= 0) 
                throw new ArgumentException(nameof(height) + " must be positive.");
            if (width <= 0)
                throw new ArgumentException(nameof(width) + " must be positive.");
            PrepareSuperPositions();
        }

        private void PropagateFieldChange()
        {
            while (fieldStack.Count != 0)
            {
                SuperpositionField field = fieldStack.Pop();
                //Console.WriteLine("Propagated to (" + field.Coordinate.X + ", " + field.Coordinate.Y + ")" );
                Dictionary<Direction, Rule> rfc = field.RuleForCell.Rules;
                Coordinate c = field.Coordinate;
                foreach (var item in rfc)
                {
                  Direction dir = item.Key;
                  Rule rule = item.Value;
                  try
                  {
                      Coordinate c2 = c.Step(dir);
                      int x = c2.X;
                      int y = c2.Y;

                      int i = x * Height + y;
                      SuperpositionField chosenField = fields[i];
                      bool modified = chosenField.Restrict(rule); 
                      if (modified) fieldStack.Push(chosenField);
                  }
                  catch (ArgumentOutOfRangeException e)
                  {
                  }
                }

            }
        }
        private bool Iter()
        {
            List<SuperpositionField> notCollapsed = fields.FindAll(s => !s.Collapsed);
            if (notCollapsed.Count == 0) return true;
            //Debug.Assert(collapsed+notCollapsed.Count == fields.Count);
            int minEntropy = notCollapsed.Min(s => s.Entropy);
            //Debug.Assert(minEntropy > 1);
            List<SuperpositionField> fieldsToCollapse = fields.Where(s => s.Entropy == minEntropy).ToList();
            int len = fieldsToCollapse.Count;
            int i = random.Next(len);
            SuperpositionField chosenField = fieldsToCollapse[i];
            RuleForCell collapsedRules = chosenField.Collapse(random);
            fieldStack = new();
            fieldStack.Push(chosenField);
            PropagateFieldChange();

            return false;


        }


        public List<List<Field>> Generate()
        {
            try
            {
                while (!Iter())
                {
                    
                }
                return ToFields();
            }
            catch (MapGenerationConflictException e)
            {
                Console.WriteLine(e);
                
                triedTimes++;
                if (triedTimes < maxTriedTimes)
                    return Generate();
            }
/*
            
            return f;

        */
            return [];


        }

        private List<List<Field>> ToFields()
        {
            Console.Write(ToString());
            List<List<Field>> f = new List<List<Field>>();

            for (int i = 0; i < fields.Count; i++)
            {
                int x = i / Width;
                int y = i % Width;
                if (y == 0) f.Add(new List<Field>());
                f[x].Add(fields[i].ToField());
            }

            return f;
        }

        private void PrepareSuperPositions()
        {
            fields = new List<SuperpositionField>();
            for (int x = 0; x < Width; x++)
            {
            
                for (int y = 0; y < Height; y++)
                {
                    fields.Add(new SuperpositionField(RuleForCell.All(), new Coordinate(x, y), ruleBook));
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Width; i++)
            {
                
                for (int j = 0; j < Height; j++)
                {
                    sb.Append(fields[j*Width+i]);
                    //sb.Append(";");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}

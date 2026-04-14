
namespace CSDB_UtopiaModel.Persistence.MapGeneration
{
    internal class RuleForCell
    {

        private Dictionary<Direction, Rule> rules;
        public Dictionary<Direction, Rule> Rules {get => DirCopy(rules);}
        public Rule GetRule(Direction d)
        {
            return rules[d].Copy();
        }

        public RuleForCell(RuleForCell other)
        {
            rules = DirCopy(other.rules);
        }

        private static Dictionary<Direction, Rule> DirCopy(Dictionary<Direction, Rule> r)
        {
            Dictionary<Direction, Rule> a = new();
            foreach (var item in r)
            {
                a.Add(item.Key, item.Value.Copy());
            }

            return a;
        }

        public RuleForCell Copy()
        {
            return new RuleForCell(this);
        }
        public RuleForCell(Dictionary<Direction, Rule> rules)
        {
            this.rules = rules.ToDictionary<Direction, Rule>();
        }

        public RuleForCell(): this(new Rule(), new Rule(),  new Rule(), new Rule()) {}

        public RuleForCell(Rule rulesUp, Rule rulesDown, Rule rulesLeft, Rule rulesRight)
        {
            this.rules = new();
            this.rules[UP.Instance()] =  rulesUp;
            this.rules[DOWN.Instance()] =  rulesDown;
            this.rules[LEFT.Instance()] =  rulesLeft;
            this.rules[RIGHT.Instance()] =  rulesRight;
        }

        public static RuleForCell All()
        {
            return new RuleForCell(Rule.All(), Rule.All(),  Rule.All(), Rule.All());
        }
        public static RuleForCell Without(Rule cmpltRuleUp, Rule cmpltRuleDown, Rule cmpltRuleLeft, Rule cmpltRuleRight)
        {
            Rule up = Rule.AllExcept(cmpltRuleUp);
            Rule down = Rule.AllExcept(cmpltRuleDown);
            Rule left = Rule.AllExcept(cmpltRuleLeft);
            Rule right = Rule.AllExcept(cmpltRuleRight);
            return new RuleForCell(up, down, left, right);
        }

        public RuleForCell Except(RuleForCell other)
        {
            Dictionary<Direction, Rule> otherRules = new Dictionary<Direction, Rule>();
            foreach (var item in this.rules)
            {
                Direction dir =  item.Key;
                Rule rule = item.Value.Except(other.GetRule(dir));
                otherRules.Add(dir, rule.Copy());
            }
            return new RuleForCell(otherRules);
        }


        public void UnionWith(RuleForCell other)
        {
            foreach (Direction dir in this.rules.Keys)
            {
                Rule otherRule = other.rules[dir];
                this.rules[dir].UnionWith(otherRule);
                
            }
        }
        public void IntersectWith(RuleForCell other)
        {
            foreach (Direction dir in this.rules.Keys)
            {
                Rule otherRule = other.rules[dir];
                this.rules[dir].IntersectWith(otherRule);
                
            }
        }
    }
}

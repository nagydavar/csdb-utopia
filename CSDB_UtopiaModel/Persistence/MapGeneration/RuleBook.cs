
using System.Diagnostics;

namespace CSDB_UtopiaModel.Persistence.MapGeneration; 

internal class RuleBook
{
    private Dictionary<FieldTypes, RuleForCell> rules = new Dictionary<FieldTypes, RuleForCell>();

    
    public RuleBook()
    { 
        Rule allRoad = new([FieldTypes.RoadHor, FieldTypes.RoadVer, FieldTypes.FourInter, FieldTypes.ThreeInterHorUp, FieldTypes.ThreeInterHorDown,
            FieldTypes.ThreeInterVerRight, FieldTypes.ThreeInterVerLeft]);
        Rule allInter = new Rule([FieldTypes.FourInter, FieldTypes.ThreeInterHorUp, FieldTypes.ThreeInterHorDown,
            FieldTypes.ThreeInterVerRight, FieldTypes.ThreeInterVerLeft]);  
        foreach (var field in Enum.GetValues<FieldTypes>())
        {
            
            Rule u = Rule.All();
            Rule d = Rule.All();
            Rule l = Rule.All();
            Rule r = Rule.All();
            if (field == FieldTypes.RoadHor)
            {
                u = u.Except(allRoad);
                d = d.Except(allRoad);
                l = allRoad.Except(new Rule([FieldTypes.RoadVer, FieldTypes.ThreeInterVerLeft]));
                r = allRoad.Except(new Rule([FieldTypes.RoadVer, FieldTypes.ThreeInterVerRight]));
                
            }

            else if (field == FieldTypes.RoadVer)
            {
                
               u = allRoad.Except(new Rule([FieldTypes.RoadHor, FieldTypes.ThreeInterHorUp]));
               d = allRoad.Except(new Rule([FieldTypes.RoadHor, FieldTypes.ThreeInterHorDown]));
               l = l.Except(allRoad);
               r = r.Except(allRoad);

            }else if (field == FieldTypes.FourInter)
            {
                
                u = new Rule(FieldTypes.RoadVer);
                d = new Rule(FieldTypes.RoadVer);
                l = new Rule(FieldTypes.RoadHor);
                r = new Rule(FieldTypes.RoadHor);
            }else if (field == FieldTypes.ThreeInterHorUp)
            {
                
                u = new Rule(FieldTypes.RoadVer);
                d = d.Except(allRoad);
                l = new Rule(FieldTypes.RoadHor);
                r = new Rule(FieldTypes.RoadHor);
                
            }else if (field == FieldTypes.ThreeInterHorDown)
            {
                u = u.Except(allRoad);
                d = new Rule(FieldTypes.RoadVer);
                l = new Rule(FieldTypes.RoadHor);
                r = new Rule(FieldTypes.RoadHor);
            }
            else if (field == FieldTypes.ThreeInterVerLeft)
            {
                
                u = new Rule(FieldTypes.RoadVer);
                d = new Rule(FieldTypes.RoadVer);
                l = new Rule(FieldTypes.RoadHor);
                r = r.Except(allRoad);
            }
            else if (field == FieldTypes.ThreeInterVerRight)
            {
                u = new Rule(FieldTypes.RoadVer);
                d = new Rule(FieldTypes.RoadVer);
                l = l.Except(allRoad);
                r = new Rule(FieldTypes.RoadHor);
            }
            else
            {

                u = new Rule([
                    FieldTypes.Land, FieldTypes.Forest, FieldTypes.Water, FieldTypes.Mountain, FieldTypes.ThreeInterHorUp,
                    FieldTypes.RoadHor
                ]);
                d = new Rule([
                    FieldTypes.Land, FieldTypes.Forest, FieldTypes.Water, FieldTypes.Mountain, FieldTypes.ThreeInterHorDown,
                    FieldTypes.RoadHor
                ]);
                l = new Rule([
                    FieldTypes.Land, FieldTypes.Forest, FieldTypes.Water, FieldTypes.Mountain, FieldTypes.ThreeInterVerLeft,
                    FieldTypes.RoadVer
                ]);
                r = new Rule([
                    FieldTypes.Land, FieldTypes.Forest, FieldTypes.Water, FieldTypes.Mountain, FieldTypes.ThreeInterVerRight,
                    FieldTypes.RoadVer
                ]);
            }
            RuleForCell rfc = new RuleForCell(u, d, l, r);
            rules.Add(field, rfc);
        }

        foreach (var a in Enum.GetValues<FieldTypes>())
        {
            
            foreach (var b in Enum.GetValues<FieldTypes>())
            {
                List<Direction> directions = new List<Direction>([UP.Instance(), DOWN.Instance(), LEFT.Instance(), RIGHT.Instance()]);
                foreach (Direction d in directions)
                {
                    bool bINa = rules[a].GetRule(d).Contains(b);
                    bool aINb = rules[b].GetRule(d.Opposite()).Contains(a);
                    Debug.Assert(aINb == bINa, "Rulebook inconsistency");
                }
            }
        }

      
    }
    public RuleForCell Get(FieldTypes field)
    {
        return rules[field].Copy();
    }
}
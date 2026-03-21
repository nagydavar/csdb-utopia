
using System.Diagnostics;

namespace CSDB_UtopiaModel.Persistence.MapGeneration; 

internal class RuleBook
{
    private Dictionary<FieldTypes, RuleForCell> rules = new Dictionary<FieldTypes, RuleForCell>();

    
    public RuleBook()
    { 
        Rule allRoad = new([FieldTypes.ROAD_HOR, FieldTypes.ROAD_VER, FieldTypes.FOUR_INTER, FieldTypes.THREE_INTER_HOR_UP, FieldTypes.THREE_INTER_HOR_DOWN,
            FieldTypes.THREE_INTER_VER_RIGHT, FieldTypes.THREE_INTER_VER_LEFT]);
        Rule allInter = new Rule([FieldTypes.FOUR_INTER, FieldTypes.THREE_INTER_HOR_UP, FieldTypes.THREE_INTER_HOR_DOWN,
            FieldTypes.THREE_INTER_VER_RIGHT, FieldTypes.THREE_INTER_VER_LEFT]);  
        foreach (var field in Enum.GetValues<FieldTypes>())
        {
            
            Rule u = Rule.All();
            Rule d = Rule.All();
            Rule l = Rule.All();
            Rule r = Rule.All();
            if (field == FieldTypes.ROAD_HOR)
            {
                u = u.Except(allRoad);
                d = d.Except(allRoad);
                l = allRoad.Except(new Rule([FieldTypes.ROAD_VER, FieldTypes.THREE_INTER_VER_LEFT]));
                r = allRoad.Except(new Rule([FieldTypes.ROAD_VER, FieldTypes.THREE_INTER_VER_RIGHT]));
                
            }

            else if (field == FieldTypes.ROAD_VER)
            {
                
               u = allRoad.Except(new Rule([FieldTypes.ROAD_HOR, FieldTypes.THREE_INTER_HOR_UP]));
               d = allRoad.Except(new Rule([FieldTypes.ROAD_HOR, FieldTypes.THREE_INTER_HOR_DOWN]));
               l = l.Except(allRoad);
               r = r.Except(allRoad);

            }else if (field == FieldTypes.FOUR_INTER)
            {
                
                u = new Rule(FieldTypes.ROAD_VER);
                d = new Rule(FieldTypes.ROAD_VER);
                l = new Rule(FieldTypes.ROAD_HOR);
                r = new Rule(FieldTypes.ROAD_HOR);
            }else if (field == FieldTypes.THREE_INTER_HOR_UP)
            {
                
                u = new Rule(FieldTypes.ROAD_VER);
                d = d.Except(allRoad);
                l = new Rule(FieldTypes.ROAD_HOR);
                r = new Rule(FieldTypes.ROAD_HOR);
                
            }else if (field == FieldTypes.THREE_INTER_HOR_DOWN)
            {
                u = u.Except(allRoad);
                d = new Rule(FieldTypes.ROAD_VER);
                l = new Rule(FieldTypes.ROAD_HOR);
                r = new Rule(FieldTypes.ROAD_HOR);
            }
            else if (field == FieldTypes.THREE_INTER_VER_LEFT)
            {
                
                u = new Rule(FieldTypes.ROAD_VER);
                d = new Rule(FieldTypes.ROAD_VER);
                l = new Rule(FieldTypes.ROAD_HOR);
                r = r.Except(allRoad);
            }
            else if (field == FieldTypes.THREE_INTER_VER_RIGHT)
            {
                u = new Rule(FieldTypes.ROAD_VER);
                d = new Rule(FieldTypes.ROAD_VER);
                l = l.Except(allRoad);
                r = new Rule(FieldTypes.ROAD_HOR);
            }
            else
            {

                u = new Rule([
                    FieldTypes.LAND, FieldTypes.FOREST, FieldTypes.WATER, FieldTypes.THREE_INTER_HOR_UP,
                    FieldTypes.ROAD_HOR
                ]);
                d = new Rule([
                    FieldTypes.LAND, FieldTypes.FOREST, FieldTypes.WATER, FieldTypes.THREE_INTER_HOR_DOWN,
                    FieldTypes.ROAD_HOR
                ]);
                l = new Rule([
                    FieldTypes.LAND, FieldTypes.FOREST, FieldTypes.WATER, FieldTypes.THREE_INTER_VER_LEFT,
                    FieldTypes.ROAD_VER
                ]);
                r = new Rule([
                    FieldTypes.LAND, FieldTypes.FOREST, FieldTypes.WATER, FieldTypes.THREE_INTER_VER_RIGHT,
                    FieldTypes.ROAD_VER
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
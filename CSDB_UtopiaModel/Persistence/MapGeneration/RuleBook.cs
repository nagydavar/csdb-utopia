
using System.Diagnostics;

namespace CSDB_UtopiaModel.Persistence.MapGeneration; 

internal class RuleBook
{
    protected Dictionary<FieldTypes, RuleForCell> rules = new Dictionary<FieldTypes, RuleForCell>();
    protected Dictionary<FieldTypes, int> proportions = new Dictionary<FieldTypes, int>();
    
    public RuleBook()
    { 
        proportions.Add(FieldTypes.Land, 200);
        proportions.Add(FieldTypes.Forest, 170);
        proportions.Add(FieldTypes.Water, 130);
        proportions.Add(FieldTypes.Mountain, 80);
        proportions.Add(FieldTypes.RoadVer, 30);
        proportions.Add(FieldTypes.RoadHor, 30);
        proportions.Add(FieldTypes.FourInter, 18);
        proportions.Add(FieldTypes.ThreeInterHorUp, 13);
        proportions.Add(FieldTypes.ThreeInterHorDown, 13);
        proportions.Add(FieldTypes.ThreeInterVerLeft, 13);
        proportions.Add(FieldTypes.ThreeInterVerRight, 13);
        proportions.Add(FieldTypes.BuildingUpLeft, 20);
        proportions.Add(FieldTypes.BulidingUpRight, 20);
        proportions.Add(FieldTypes.BuildingDownLeft, 20);
        proportions.Add(FieldTypes.BuildingDownRight, 20);
        Rule allRoad = new([FieldTypes.RoadHor, FieldTypes.RoadVer, FieldTypes.FourInter, FieldTypes.ThreeInterHorUp, FieldTypes.ThreeInterHorDown,
            FieldTypes.ThreeInterVerRight, FieldTypes.ThreeInterVerLeft]);
        Rule allInter = new Rule([FieldTypes.FourInter, FieldTypes.ThreeInterHorUp, FieldTypes.ThreeInterHorDown,
            FieldTypes.ThreeInterVerRight, FieldTypes.ThreeInterVerLeft]);
        Rule allBuilding = new Rule([
            FieldTypes.BuildingUpLeft, FieldTypes.BulidingUpRight, FieldTypes.BuildingDownLeft,
            FieldTypes.BuildingDownRight
        ]);
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
                ]).Union(allBuilding);
                d = new Rule([
                    FieldTypes.Land, FieldTypes.Forest, FieldTypes.Water, FieldTypes.Mountain, FieldTypes.ThreeInterHorDown,
                    FieldTypes.RoadHor
                ]).Union(allBuilding);
                l = new Rule([
                    FieldTypes.Land, FieldTypes.Forest, FieldTypes.Water, FieldTypes.Mountain, FieldTypes.ThreeInterVerLeft,
                    FieldTypes.RoadVer
                ]).Union(allBuilding);
                r = new Rule([
                    FieldTypes.Land, FieldTypes.Forest, FieldTypes.Water, FieldTypes.Mountain, FieldTypes.ThreeInterVerRight,
                    FieldTypes.RoadVer
                ]).Union(allBuilding);
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
    public FieldTypes ProportionalRandom(Random r, List<FieldTypes> possibleFields)
    {
        int sum = possibleFields.Sum(s => GetProportion(s));
        int rand = r.Next(sum);
        int currentSum = 0;
        for (int i = 0; i < possibleFields.Count; i++)
        {
            currentSum += GetProportion(possibleFields[i]);
            if  (rand < currentSum) return possibleFields[i];
        }
        throw new MapGenerationConflictException();
            
    }
    public RuleForCell Get(FieldTypes field)
    {
        return rules[field].Copy();
    }

    public int GetProportion(FieldTypes field) => proportions.ContainsKey(field) ? proportions[field] : 0;

}
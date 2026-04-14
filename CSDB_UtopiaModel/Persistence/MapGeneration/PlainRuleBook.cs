namespace CSDB_UtopiaModel.Persistence.MapGeneration;

class PlainRuleBook:RuleBook
{
    public PlainRuleBook() : base()
    {
        proportions[FieldTypes.BuildingDownLeft] = 0;
        proportions[FieldTypes.BuildingDownRight] = 0;
        proportions[FieldTypes.BuildingUpLeft] = 0;
        proportions[FieldTypes.BulidingUpRight] = 0;
        
        
        proportions[FieldTypes.RoadHor] = 0;
        proportions[FieldTypes.RoadVer] = 0;
        proportions[FieldTypes.FourInter] = 0;
        
        proportions[FieldTypes.ThreeInterHorDown] = 0;
        proportions[FieldTypes.ThreeInterHorUp] = 0;
        proportions[FieldTypes.ThreeInterVerLeft] = 0;
        proportions[FieldTypes.ThreeInterVerRight] = 0;
        
        base.updatePossibleFields();
        
    }
}
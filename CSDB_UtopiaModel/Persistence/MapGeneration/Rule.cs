namespace CSDB_UtopiaModel.Persistence.MapGeneration;

internal class Rule:  HashSet<FieldTypes>
{
    public Rule():base() {}
    public Rule(FieldTypes f):base([f]) {}

    public object Clone() => new Rule(this);

    public Rule Copy()
    {
        return new Rule(this);
    }
    
    public Rule(IEnumerable<FieldTypes> fields) : base(fields){}
    public static Rule All()
    {
        return new Rule(Enum.GetValues<FieldTypes>());
    }

    public static Rule None() => new Rule();

    public static Rule AllExcept(IEnumerable<FieldTypes> rules)
    {
        return new Rule(rules.Except(Enum.GetValues<FieldTypes>()));
    }

    public Rule Except(HashSet<FieldTypes> rule)
    {
        HashSet<FieldTypes> f = this.Copy();
        f.ExceptWith(rule);
        return new Rule(f);
    }
    public Rule Union(HashSet<FieldTypes> rule)
    {
        HashSet<FieldTypes> f = this.Copy();
        f.UnionWith(rule);
        return new Rule(f);
    }

}
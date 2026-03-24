namespace CSDB_UtopiaModel.Persistence.MapGeneration;

class GenerateTown
{
    public string Name { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    private Generator generator;
    

    public GenerateTown(int width, int height, string name)
    {
        Name = name;
        Width = width;
        Height = height;
        generator = new Generator(width, height, new RuleBook());
    }
    
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Town {
    private int level;
    private string name;
    private List<Field> partOfTown = new();
    public int GetLevel;
    
    // Property a UI számára
    public string Name => name;

    public Town(string nev)
    {
        name = nev;
    }
    public Town() { }
    public Town(List<Field> lista, string nev) { }
    public void AddToTown(Field field)
    {
        if (!partOfTown.Contains(field))
            partOfTown.Add(field);
    }
    public void Expand() { }
    };

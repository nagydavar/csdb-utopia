using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;

abstract public class Field
{
    protected Resource resource;
    protected int depletionLevel;
    protected Town? town;
    public Buildable? Buildable { get; internal set; } // model requires setter
    protected int mood;
    public Coordinate Coordinates { get; private set; }
    
    public bool HasBuildable => Buildable is not null;
    public bool IsPartOfTown=> town is not null;

    public Field(Coordinate c)
    {
        Coordinates = c;

        // Alapértelmezett értékek beállítása
        Buildable = null; // Kezdetben üres a mezõ
        town = null;      // Nem tartozik városhoz
        depletionLevel = 100;
        mood = 0;
        resource = Gold.Instance();
    }

    public void Place(Buildable buildable)
    {
        if (Buildable is not null)
            throw new InvalidOperationException("Can't place it here.");
        Buildable = buildable;
    }

    public bool Demolish()
    {
        return true;
    }
}
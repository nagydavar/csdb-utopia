using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;

public abstract class Field
{
    protected readonly Random _rnd = new();
    
    protected Town? town;
    public Buildable? Buildable { get; internal set; }
    public Coordinate Coordinates { get; private set; }
    public int DepletionLevel { get; internal set; }

    public IResource Resource { get; internal set; }

    public bool HasBuildable => Buildable is not null;
    public bool IsPartOfTown => town is not null;

    //Relatív pozíció a nagyobb méretű épületekhez
    public int RelativeX { get; set; } = 0;
    public int RelativeY { get; set; } = 0;

    public Field(Coordinate c)
    {
        Coordinates = c;

        // Alap�rtelmezett �rt�kek be�ll�t�sa
        Buildable = null; // Kezdetben �res a mez�
        town = null; // Nem tartozik v�roshoz
        
        IResource[] a =
        [
            Gold.Instance(), Diamond.Instance(), Wood.Instance(), IronOre.Instance(), Coal.Instance(), Oil.Instance()
        ];
        
        Resource = a[_rnd.Next(0, a.Length)];
        DepletionLevel = _rnd.Next(1, 101);
    }

    public virtual void Place(Buildable buildable)
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
using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;
public class Field {
    protected Resource resource;
    protected int depletionLevel;
    protected Town? town;
    protected Buildable? buildable;
    protected int mood;
    protected int _x;
    protected int _y;

    public int X;
    public int Y;
    public bool HasBuildable;
    public bool IsPartOfTown;
    public int MoodLevel;

    public Field(int x, int y) { }
    public bool Place(Buyable buyable) { return true; }
    public bool Demolish() {
        return true;
    }
};

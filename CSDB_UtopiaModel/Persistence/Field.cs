using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;
abstract public class Field {
    protected Resource resource;
    protected int depletionLevel;
    protected Town? town;
    protected Buildable? buildable;
    protected int mood;
    public Coordinate Coordinates { get; private set; }
    public bool HasBuildable;
    public bool IsPartOfTown;

    public Field(Coordinate c) { }

    public void Place(Buildable buildable)
    {
        this.buildable = buildable;
    }
    public bool Demolish() {
        return true;
    }
};

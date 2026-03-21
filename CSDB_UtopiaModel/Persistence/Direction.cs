namespace CSDB_UtopiaModel.Persistence;
public interface Direction {
    
    public abstract (int, int) Diff();
    public abstract Direction Opposite();
    };

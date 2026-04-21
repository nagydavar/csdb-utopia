namespace CSDB_UtopiaModel.Persistence;

public class RandomSingleton: Random
{
    public static RandomSingleton Instance { get; } = new RandomSingleton();
    private RandomSingleton():base() {}
}
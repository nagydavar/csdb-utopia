namespace CSDB_UtopiaModel.Persistence;
public class Jewelry : Goods
{
    private static Jewelry? instance;
    private Jewelry() { }
    public static Jewelry Instance()
    {
        if (instance is null)
            instance = new Jewelry();
        return instance;
    }
};

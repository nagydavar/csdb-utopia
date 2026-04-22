using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Building : Buildable
{
    protected Building(Field f) : base(f)
    {
    }
}
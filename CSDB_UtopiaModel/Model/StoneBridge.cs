using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class StoneBridge : Bridge
{
    public StoneBridge(Field f, Direction d) : base(f, 20, d)
    {
        //TODO value of maxSpeed
    }
}
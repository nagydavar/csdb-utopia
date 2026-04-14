using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class SteelBridge : Bridge
{
    public SteelBridge(Field f, Direction d) : base(f, 30, d)
    {
        //TODO value of maxSpeed
    }
}
using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class WoodenBridge : Bridge
{
    //kitalalt ertek a 10
    public WoodenBridge(Field f, IDirection d) : base(f, 10, d) {}
}

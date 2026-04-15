using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

class StoneBridge : Bridge
{
    //kitalalt ertek a 30
    public StoneBridge(Field f, IDirection d) : base(f, 20, d) {}
};

using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class SteelBridge: Bridge
{
    //kitalalt ertek a 30
    public SteelBridge(Field f, IDirection d) : base(f, 30, d) {}
        
};

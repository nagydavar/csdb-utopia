using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
abstract class Buildable : Buyable
{
        protected int placementCost;
        protected Pair<int,int> area;
        protected Field owner;
        protected Coords field;
;

    };

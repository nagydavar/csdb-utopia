using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;
public class Land : Field
{

        public int LevelOfForest { get; private set; }
        public bool CanGrow { get; private set; }

        public Land(Coordinate c, int forestLevel, bool canGrow): base(c)
        {
            LevelOfForest = forestLevel;
            CanGrow = canGrow;
        }

        public void ForestSpread()
        {
            throw new NotImplementedException();
        }
    };

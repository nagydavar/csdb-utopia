namespace CSDB_UtopiaModel.Persistence;
public class Land : Field
{
        private int forestLevel;

        public int LevelOfForest;
        public bool CanGrow;
        public void Land(int,int);
        public void ForestSpread();
    };

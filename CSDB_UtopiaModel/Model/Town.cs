using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class Town {
        private int level;
        private string name;
        private List<Field> partOfTown;
        public int GetLevel;

        public Town();
        public Town(string);
        public Town(List<Field>, string);
        private AddToTown(Field) void;
        public void Expand();
    };

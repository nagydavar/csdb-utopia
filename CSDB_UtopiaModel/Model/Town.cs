using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Town {
        private int level;
        private string name;
        private List<Field> partOfTown;
        public int GetLevel;

    public Town() { }
    public Town(string nev) { }
    public Town(List<Field> lista, string nev) { }
    private void AddToTown(Field field) { }
    public void Expand() { }
    };

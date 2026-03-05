namespace CSDB_UtopiaModel.Model;
class Section{
        public Coords start;
        public Coords end;
        public Section calculateNew();
        public List<Coords> coordinates;
        public bool in(Coordinate);

        public void alert();
    };

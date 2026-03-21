namespace CSDB_UtopiaModel.Model;
class Section{
        public Coordinate start;
        public Coordinate end;
        public Section calculateNew() => throw new NotImplementedException();
        public List<Coordinate> coordinates;
        public bool Contains(Coordinate c) => coordinates.Contains(c);

        public void Alert() => throw new NotImplementedException();
};

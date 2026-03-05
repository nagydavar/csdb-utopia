using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;
class Persistence{
        private Field[][] Fields;
        private List<Town> Towns;
        private List<Land> Forests;
        private List<Field> Factories;
        private List<Field> Producers;
        private List<Vehicle> VehiclesOnMap;
        public Dictionary<Resource, int> Storage;
        public DateTime Date;
        public int Width;
        public int Height;
        public int Budget;
;
        public Getters;
        public Setters;

        public void Save();
        public void Load();

    };

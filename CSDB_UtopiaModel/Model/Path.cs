namespace CSDB_UtopiaModel.Model;
class Path{
        public List<Section> Sections { get; private set; }
        public List<Navigator> navigator;
        public Navigator getNewNavigator() => throw new NotImplementedException();


    };

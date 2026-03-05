using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class ResourceEventArgs: EventArgs
{
        public Resource Resource;
        public int NewValue;
        public ResourceEventArgs(Resource, int);
    };

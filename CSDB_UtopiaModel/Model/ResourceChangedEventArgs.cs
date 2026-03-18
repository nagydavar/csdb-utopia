using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class ResourceChangedEventArgs: EventArgs
{
        public Resource Resource;
        public int NewValue;
        public ResourceChangedEventArgs(Resource resource, int newValue)
        {
            Resource = resource;
            NewValue = newValue;
        }
};

using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class ResourceChangedEventArgs: EventArgs
{
        public Resource Resource;
        public int NewValue;
        public ResourceChangedEventArgs(Resource resource, int newValue)
        {
            Resource = resource;
            NewValue = newValue;
        }
};

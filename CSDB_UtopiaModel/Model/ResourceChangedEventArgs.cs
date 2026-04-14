using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class ResourceChangedEventArgs: EventArgs
{
        public IResource resource;
        public int NewValue;
        public ResourceChangedEventArgs(IResource resource, int newValue)
        {
            this.resource = resource;
            NewValue = newValue;
        }
}
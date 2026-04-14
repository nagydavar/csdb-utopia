using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public class ResourceChangedEventArgs: EventArgs
{
        public IResource Resource { get; init; }
        public int NewValue { get; init; }
        
        public ResourceChangedEventArgs(IResource resource, int newValue)
        {
            Resource = resource;
            NewValue = newValue;
        }
}
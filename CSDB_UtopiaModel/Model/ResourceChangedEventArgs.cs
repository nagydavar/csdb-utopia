using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class ResourceChangedEventArgs: EventArgs
{
        public IResource iResource;
        public int NewValue;
        public ResourceChangedEventArgs(IResource iResource, int newValue)
        {
            this.iResource = iResource;
            NewValue = newValue;
        }
};

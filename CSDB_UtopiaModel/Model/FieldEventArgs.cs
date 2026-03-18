using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
class FieldEventArgs: EventArgs
{
        public List<Field> Fields;
    public FieldEventArgs(List<Field> fields) { }
    };

using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
abstract public class Factory : Producer
{
    public Factory(Field field, int yield) : base(field, yield) { area = (2, 2); }

};

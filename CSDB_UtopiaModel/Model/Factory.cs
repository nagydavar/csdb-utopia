using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Factory : Producer
{
    public Factory(Field field, int yield, Model model) : base(field, yield, model)
    {
        area = (2, 2);
    }

}
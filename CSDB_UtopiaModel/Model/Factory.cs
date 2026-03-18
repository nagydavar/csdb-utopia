using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;
public class Factory : Producer
{
    public Factory(Field field, int x) : base(field.X,field.Y){ }

    public override Goods Produce();
    public override Resource Require();
};

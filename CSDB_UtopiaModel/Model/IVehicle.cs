using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public interface IVehicle : ITickable
{
    // TODO
    public IDirection CurrentDirection { get; set; }
}
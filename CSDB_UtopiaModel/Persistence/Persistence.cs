using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaModel.Persistence;
class Persistence
{
    public List<List<Field>> Fields { get; private set; }
    public List<Town> Towns { get; private set; }
    public List<Land> Forests { get; private set; }
    public List<Factory> Factories { get; private set; }
    public List<ResourceExtractor> ResourceExtractors { get; private set; }
    public List<Field> FactoryFields { get; private set; }
    public List<Field> ResourceExtractorFields { get; private set; }
    public List<Vehicle<Resource>> VehiclesOnMap { get; private set; }
    public Dictionary<Resource, int> Storage { get; private set; }
    public DateTime Date { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Budget { get; private set; }

    public Persistence(int width, int height)
    {
        Width = width;
        Height = height;

        Fields = new List<List<Field>>();
        for (int i = 0; i < Width; i++)
        {

            for (int j = 0; j < Height; j++)
            {
                Fields[i][j] = new Land();
            }
        }
    }

    public Persistence GenerateMap(int width, int height)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }
    public void Load()
    {
        throw new NotImplementedException();
    }

};

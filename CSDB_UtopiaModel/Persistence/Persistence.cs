using CSDB_UtopiaModel.Model;
using System;

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

    public int CurrentMood { get; set; }

    public Persistence(int width, int height)
    {
        Width = width;
        Height = height;
        Budget = 10000;
        CurrentMood = 100;

        Fields = new List<List<Field>>();
        for (int i = 0; i < Width; i++)
        {

            for (int j = 0; j < Height; j++)
            {
                Fields[i][j] = new Land(new Coordinate(i, j),0, false);
            }
        }

        Storage = new Dictionary<Resource, int>
        {
            { HumanResource.Instance(), 0 },
          
            // Ipari nyersanyagok
            { Wood.Instance(), 0 },
            { IronOre.Instance(), 0 },
            { Coal.Instance(), 0 },
            { Oil.Instance(), 0 },

            // Kincsek
            { Gold.Instance(), 0 },
            { Diamond.Instance(), 0 },

            // K�szterm�kek / Goods
            { Plank.Instance(), 0 },
            { Iron.Instance(), 0 },
            { Gasoline.Instance(), 0 },
            { Jewelry.Instance(), 0 },
            { Paper.Instance(), 0 },
            { Book.Instance(), 0 }
        };
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

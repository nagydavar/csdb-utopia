using CSDB_UtopiaModel.Model;
using System;
using System.Diagnostics;
using CSDB_UtopiaModel.Persistence.MapGeneration;

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
    public List<Vehicle<IResource>> VehiclesOnMap { get; private set; }
    public Dictionary<IResource, int> Storage { get; private set; }
    public DateTime Date { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Budget { get; set; }

    public int CurrentMood { get; set; }

    public Persistence(int width, int height, List<List<Field>> fields)
    {
        Fields = fields;
    }
    public Persistence(int width, int height, bool generateMap)
    {
        Width = width;
        Height = height;
        Budget = 10000;
        CurrentMood = 100;

        if (generateMap)
            GenerateMap(Width, Height);
        else {
            Fields = new List<List<Field>>();
            for (int i = 0; i < Width; i++)
            {

                for (int j = 0; j < Height; j++)
                {
                    Fields[i][j] = new Land(new Coordinate(i, j), 0, false);
                }
            }
        }

        Storage = new Dictionary<IResource, int>
        {
            { HumanResource.Instance(), 0 },

            // Ipari nyersanyagok
            { Wood.Instance(), 1000 },
            { IronOre.Instance(), 1000 },
            { Coal.Instance(), 1000 },
            { Oil.Instance(), 1000 },

            // Kincsek
            { Gold.Instance(), 1000 },
            { Diamond.Instance(), 1000 },

            // K�szterm�kek / Goods
            { Plank.Instance(), 1000 },
            { Iron.Instance(), 1000 },
            { Gasoline.Instance(), 1000 },
            { Jewelry.Instance(), 1000 },
            { Paper.Instance(), 1000 },
            { Book.Instance(), 1000 }
        };
    }

    private void GenerateMap(int width, int height)
    {
        Generator generator = new Generator(width, height, new PlainRuleBook());
        Fields = generator.Generate();
        Console.WriteLine("Generation Done");
        //Console.Write(generator.ToString());
        
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void Load()
    {
        throw new NotImplementedException();
    }

}
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CSDB_UtopiaModel.Model;
using CSDB_UtopiaModel.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Linq;

namespace CSDB_UtopiaView.ViewModels;

public partial class Cell : ObservableObject
{
    private readonly int _x;
    private readonly int _y;


    private string imagePath;
    
    [ObservableProperty]
    private IImage? image;
    private string _fileName;

    public int X => _x;
    public int Y => _y;

    [ObservableProperty]
    private IImage? _vehicleImage1; // 1-es sáv (pl. felfelé vagy jobbra)

    [ObservableProperty]
    private IImage? _vehicleImage2; // 2-es sáv (pl. lefelé vagy balra)

    [ObservableProperty]
    private bool _hasVehicle1;

    [ObservableProperty]
    private bool _hasVehicle2;

    public Cell(int x, int y)
    {
        _x = x;
        _y = y;
        Image = ImageLoader.GetDefault();
    }

    // A Model_FieldsUpdated hívja meg a ViewModel-ben
    public void Update(Field field)
    {
        if (field.HasBuildable)
        {
            string typeName = field.Buildable!.GetType().Name;

            // Ha az épület területe > 1x1, akkor szeletelt képet keresünk
            if (field.Buildable.area.Item1 > 1 || field.Buildable.area.Item2 > 1)
            {
                if (field.Buildable is ResourceExtractor)
                {
                    _fileName = $"Buildings/ResourceExtractors/{typeName}_{field.RelativeX}_{field.RelativeY}.PNG";
                }
                else if (field.Buildable is Decoration)
                {
                    _fileName = $"Buildings/Decorations/{typeName}_{field.RelativeX}_{field.RelativeY}.png";
                }
                else if (field.Buildable is Factory) {
                    _fileName = $"Buildings/Factories/{ typeName}_{ field.RelativeX}_{ field.RelativeY}.PNG";
                }
            }
            else
            {
                //1x1-es mezők

                if (field.Buildable is ApartmentBlock)
                    _fileName = $"Buildings/ResidentialBuilding/{typeName}.PNG";
                else if (field.Buildable is DetachedHouse)
                    _fileName = $"Buildings/ResidentialBuilding/{typeName}.jpg";
                else if (field.Buildable is Decoration)
                    _fileName = $"Buildings/Decorations/{typeName}.png";
                else if (field.Buildable is Garage || field.Buildable is Stop)
                {
                    _fileName = $"Buildings/OtherBuildings/{typeName}.PNG";
                }
                else if (field.Buildable is Road road)
                {
                    // Ha Motorway-ről van szó, ellenőrizzük az elágazást
                    if (road is Motorway motorway && motorway.HasIntersection)
                    {
                        // Megkeressük a motorway-ben tárolt kereszteződést
                        
                        //kell getter vagy property
                        var intersection = motorway.GetIntersection();

                        if (intersection is FourWayIntersection)
                        {
                            // Négyágú kereszteződés (X)
                            _fileName = "Roads/Intersection_4.PNG";
                        }
                        else if (intersection is ThreeWayIntersection tWay)
                        {
                            // T-elágazás (3 irány)
                            // Itt a tWay.Direction (TrafficLightIDirection) adja meg a T szárának irányát
                            string dirSuffix = GetDirectionSuffix(tWay.TrafficLightIDirection);
                            _fileName = $"Roads/Intersection_3_{dirSuffix}.PNG";
                        }
                    }
                    else
                    {
                        // Sima egyenes út (H vagy V) vagy kanyar

                        // Tegyük fel, hogy a road.GetConnections() visszaad egy listát: List<IDirection>
                        var connections = GetRoadConnections(field);

                        if (connections.Count == 2)
                        {
                            string curveSuffix = GetCurveSuffix(connections);
                            if (!string.IsNullOrEmpty(curveSuffix))
                            {
                                // Kanyar képek Roads/Motorway_Curve_LU.PNG (Left-Up), stb.
                                _fileName = $"Roads/{typeName}_Curve_{curveSuffix}.PNG";
                            }
                            else
                            {
                                // Ha nem kanyar, akkor egyenes (H vagy V)
                                string directionSuffix = road.Direction is IHorizontalDirection ? "H" : "V";
                                _fileName = $"Roads/{typeName}_{directionSuffix}.PNG";
                            }
                        }
                        else
                        {
                            // Alapértelmezett egyenes
                            string directionSuffix = road.Direction is IHorizontalDirection ? "H" : "V";
                            _fileName = $"Roads/{typeName}_{directionSuffix}.PNG";
                        }
                    }
                }
            }
        }
        else
        {
            if (field is Land land)
            {
                _fileName = $"Fields/{land.LevelOfForest}trees.PNG";
            }
            else if (field is Mountain)
            {
                _fileName = "Fields/Mountain.PNG";
            }
            else if (field is Water)
            {
                _fileName = "Fields/Water.PNG";
            }
            else
            {
                _fileName = "Fields/0trees.PNG";
            }
        }
        Image = ImageLoader.Get(_fileName);


        UpdateVehicles(field);
    }

    private string GetDirectionSuffix(IDirection dir)
    {
        // A típusnév alapján (pl. Up, Down, Left, Right osztályok neve)
        // Ha a dir null, adjunk vissza egy alapértelmezettet
        return dir?.GetType().Name ?? "Up";
    }

    // Megvizsgálja, hogy a 2 csatlakozási irány kanyart alkot-e
    private string GetCurveSuffix(List<IDirection> connections)
    {
        // Megnézzük a típusokat (pl. Up, Down, Left, Right osztályok)
        bool hasUp = connections.Any(d => d is Up);
        bool hasDown = connections.Any(d => d is Down);
        bool hasLeft = connections.Any(d => d is Left);
        bool hasRight = connections.Any(d => d is Right);

        if (hasLeft && hasUp) return "LU";    // Bal-Fel kanyar
        if (hasRight && hasUp) return "RU";   // Jobb-Fel kanyar
        if (hasLeft && hasDown) return "LD";  // Bal-Le kanyar
        if (hasRight && hasDown) return "RD"; // Jobb-Le kanyar

        return string.Empty; // Egyenes út (pl. Up-Down vagy Left-Right)
    }

    // Ez a függvény a modelltől kérdezi meg, kik a szomszédos utak
    private List<IDirection> GetRoadConnections(Field currentField)
    {
        // Ideális esetben a Motorway tudja a szomszédait.
        // Ha nem, akkor a Modell referenciáján keresztül kell lekérdezni 
        // a 4 szomszédos koordinátát és megnézni, van-e ott út.

        // ha a Road tárolja a Section-öket:
        // return currentField.Buildable.Sections.Select(s => s.Direction).ToList();

        return new List<IDirection>(); // Implementálandó a Modell felépítése alapján
    }

    private void UpdateVehicles(Field field)
    {
        // Ez egy példa logika, a Modell felépítésétől függően:
        if (field.Buildable is Road e)
        { 
            HasVehicle1 = e.RightSide != null;
            HasVehicle2 = e.LeftSide != null;

            //TODO ha már van a modellben implementálva az út. autók
            if (HasVehicle1)
            {
                //    VehicleImage1 = ImageLoader.Get($"Vehicles/{e.RightSide.Type}.PNG");
            }

            if (HasVehicle2)
            {
                //    VehicleImage2 = ImageLoader.Get($"Vehicles/{e.LeftSide.Type}.PNG");
            }
        }
    }
}
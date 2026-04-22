using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CSDB_UtopiaModel.Model;
using CSDB_UtopiaModel.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    [ObservableProperty]
    private string? _fileName;

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

    [ObservableProperty]
    private double _vehicle1Rotation; // Fokokban (0, 90, 180, 270)

    [ObservableProperty]
    private double _vehicle2Rotation;

    [ObservableProperty]
    private bool _isSelected;

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
                    FileName = $"Buildings/ResourceExtractors/{typeName}_{field.RelativeX}_{field.RelativeY}.PNG";
                }
                else if (field.Buildable is Decoration)
                {
                    FileName = $"Buildings/Decorations/{typeName}_{field.RelativeX}_{field.RelativeY}.png";
                }
                else if (field.Buildable is Factory) {
                    FileName = $"Buildings/Factories/{ typeName}_{ field.RelativeX}_{ field.RelativeY}.PNG";
                }
            }
            else
            {
                //1x1-es mezők

                if (field.Buildable is ApartmentBlock)
                    FileName = $"Buildings/ResidentialBuilding/{typeName}.PNG";
                else if (field.Buildable is DetachedHouse)
                    FileName = $"Buildings/ResidentialBuilding/{typeName}.jpg";
                else if (field.Buildable is Decoration)
                    FileName = $"Buildings/Decorations/{typeName}.png";
                else if (field.Buildable is Garage || field.Buildable is Stop)
                {
                    FileName = $"Buildings/OtherBuildings/{typeName}.PNG";
                }
                else if (field.Buildable is Bridge bridge)
                {
                    if (bridge.IsCurved)
                    {
                        FileName = $"Roads/{typeName}_Curve_{bridge.Quadrant}.PNG";
                    }
                    else
                    {
                        // A hidak alapvetően egyenesek (Direction alapján)
                        // dType: V (Vertical) vagy H (Horizontal)
                        bool isVertical = bridge.Direction is Up || bridge.Direction is Down;
                        string dirType = isVertical ? "V" : "H";

                        FileName = $"Roads/{typeName}_{dirType}.PNG";
                    }
                }
                else if (field.Buildable is Road road)
                {
                    if (road is Motorway motorway && motorway.HasIntersection)
                    {
                        var intersection = motorway.GetIntersection();
                        if (intersection is FourWayIntersection)
                        {
                            FileName = "Roads/Intersection_4.PNG";
                        }
                        else if (intersection is ThreeWayIntersection tWay)
                        {
                            // A Direction adja meg, merre néz a T-elágazás szára (Up, Down, Left, Right)
                            string dirSuffix = GetDirectionSuffix(tWay.TrafficLightIDirection);
                            FileName = $"Roads/Intersection_3_{dirSuffix}.PNG";
                        }
                    }
                    else if (road.IsCurved)
                    {
                        // A Modell Quadrant értéke alapján (1, 2, 3, 4)
                        // 1: Jobb-Fel, 2: Bal-Fel, 3: Bal-Le, 4: Jobb-Le
                        FileName = $"Roads/{typeName}_Curve_{road.Quadrant}.PNG";
                    }
                    else
                    {
                        // Egyenes út: Ha a Direction Up vagy Down, akkor V (Vertical), különben H
                        bool isVertical = road.Direction is Up || road.Direction is Down;
                        string dirType = isVertical ? "V" : "H";
                        FileName = $"Roads/{typeName}_{dirType}.PNG";
                    }
                }
            }
        }
        else
        {
            if (field is Land land)
            {
                FileName = $"Fields/{land.LevelOfForest}trees.PNG";
            }
            else if (field is Mountain)
            {
                FileName = "Fields/Mountain.PNG";
            }
            else if (field is Water)
            {
                FileName = "Fields/Water.PNG";
            }
            else
            {
                FileName = "Fields/0trees.PNG";
            }
        }
        Image = ImageLoader.Get(FileName);


        UpdateVehicles(field);
    }

    private string GetDirectionSuffix(IDirection dir)
    {
        if (dir is Up) return "Up";
        if (dir is Down) return "Down";
        if (dir is Left) return "Left";
        if (dir is Right) return "Right";
        return "Up"; // Alapértelmezett
    }

    private void UpdateVehicles(Field field)
    {
        if (field.Buildable is Road road)
        {
            IVehicle? rightSide = road.RightSide;
            // 1. sáv ellenőrzése
            if (rightSide is not null)
            {
                HasVehicle1 = true;
                Vehicle1Rotation = GetRotationAngle(rightSide.Intention.To);
                // Kinyerjük a típusnevet és levágjuk a generikus jelölőt ha van
                string vType = rightSide.GetType().Name.Split('`')[0];
                VehicleImage1 = ImageLoader.Get($"Vehicles/{vType}.png");
            }
            else
            {
                HasVehicle1 = false;
                VehicleImage1 = null;
            }
            
            IVehicle? leftSide = road.LeftSide;
            // 2. sáv ellenőrzése
            if (leftSide is not null)
            {
                HasVehicle2 = true;
                Vehicle2Rotation = GetRotationAngle(leftSide.Intention.To);

                string vType = leftSide.GetType().Name.Split('`')[0];
                VehicleImage2 = ImageLoader.Get($"Vehicles/{vType}.png");
            }
            else
            {
                HasVehicle2 = false;
                VehicleImage2 = null;
            }
        }
        else
        {
            HasVehicle1 = false;
            HasVehicle2 = false;
        }
    }

    private double GetRotationAngle(IDirection dir)
    {
        if (dir is Up) return 0;
        if (dir is Right) return 90;
        if (dir is Down) return 180;
        if (dir is Left) return 270;
        return 0;
    }
}
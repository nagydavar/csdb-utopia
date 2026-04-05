using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CSDB_UtopiaModel.Model;
using CSDB_UtopiaModel.Persistence;
using System;
using System.IO;
using System.Net.NetworkInformation;

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
                    _fileName = $"Buildings/Decorations/{typeName}_{field.RelativeX}_{field.RelativeY}.PNG";
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
                    _fileName = $"Buildings/Decorations/{typeName}.PNG";
                else if (field.Buildable is Garage || field.Buildable is Stop)
                {
                    _fileName = $"Buildings/OtherBuildings/{typeName}.PNG";
                }
                else if (field.Buildable is Road) {
                    _fileName = $"Roads/{typeName}.PNG";
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
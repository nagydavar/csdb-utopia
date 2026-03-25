using System;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CSDB_UtopiaModel.Persistence;
using CSDB_UtopiaModel.Model;

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
            // Itt érdemes a Buildable-nek is egy ImagePath property-t adni, 
            // vagy típus alapján dönteni:
            if (field.Buildable is ApartmentBlock)
                _fileName = "Buildings/ResidentialBuilding/Apartment.PNG";
            else if (field.Buildable is DetachedHouse)
                _fileName = "Buildings/ResidentialBuilding/DetachedHouse.jpg";
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
        
        
        
        
        //itt folyt köv.
    }
}
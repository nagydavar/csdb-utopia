using CommunityToolkit.Mvvm.ComponentModel;
using CSDB_UtopiaModel.Persistence;
using CSDB_UtopiaModel.Model;

namespace CSDB_UtopiaView.ViewModels;

public partial class Cell : ObservableObject
{
    private readonly int _x;
    private readonly int _y;

    string baseUrl = "avares://CSDB_UtopiaView/Assets/";
    string fileName = "Fields/0trees.PNG";

    [ObservableProperty]
    private string _imagePath = "Assets/Fields/0trees.PNG"; // Alapértelmezett kép

    public int X => _x;
    public int Y => _y;

    public Cell(int x, int y)
    {
        _x = x;
        _y = y;
    }

    // A Model_FieldsUpdated hívja meg a ViewModel-ben
    public void Update(Field field)
    {
        if (field is Land land)
        {
            fileName = $"Fields/{land.LevelOfForest}trees.PNG";
        }
        else if (field is Mountain)
        {
            fileName = "Fields/Mountain.PNG";
        }
        else if (field is Water)
        {
            fileName = "Fields/Water.PNG";
        }
        else
        {
            fileName = "Fields/0trees.PNG";
        }

        ImagePath = baseUrl + fileName;
        //itt folyt köv.
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSDB_UtopiaModel.Model;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace CSDB_UtopiaView.ViewModels;

public partial class MainMenuViewModel : ViewModelBase { }
public partial class CreateWorldViewModel : ViewModelBase { }

public partial class MainViewModel : ViewModelBase
{
    private Model? _model;

    // Ez a tulajdonság tárolja az aktuálisan látható nézetet (ViewModel-t)
    [ObservableProperty]
    private ViewModelBase _currentPage;

    public MainViewModel()
    {
        // Alapértelmezett oldal a főmenü legyen
        _currentPage = new MainMenuViewModel();
    }

    // Parancsok az oldalak váltásához //TODO menteni világ nevét
    [RelayCommand]
    public void GoToCreateWorld() => CurrentPage = new CreateWorldViewModel();

    [RelayCommand]
    public void GoToGame() {
        //teszthez kisebb tábla
        int w = 50;
        int h = 50;

        _model = new Model(w, h);

        CurrentPage = new GameViewModel(w, h, _model);
    } 

    [RelayCommand]
    public void GoToMainMenu() => CurrentPage = new MainMenuViewModel();

    [RelayCommand]
    public void Quit()
    {
        // Megkeressük az aktuális alkalmazás futási környezetét
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSDB_UtopiaModel.Model;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;

namespace CSDB_UtopiaView.ViewModels;

public partial class MainMenuViewModel : ViewModelBase { }
public partial class CreateWorldViewModel : ViewModelBase { }
public partial class LoadingViewModel : ViewModelBase { }

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
    public async Task GoToGame()
    {
        // Megjelenítjük a töltőképernyőt
        CurrentPage = new LoadingViewModel();

        // Beállítjuk a méreteket
        int w = 50;
        int h = 50;

        _model = await Task.Run(() =>
        {
            var m = new Model(w, h);
            return m;
        });

        // Átváltunk a tényleges játékra
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

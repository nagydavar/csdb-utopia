using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CSDB_UtopiaView.ViewModels;

public partial class MainMenuViewModel : ViewModelBase { }
public partial class CreateWorldViewModel : ViewModelBase { }
public partial class GameViewModel : ViewModelBase { }


public partial class MainViewModel : ViewModelBase
{
    // Ez a tulajdonság tárolja az aktuálisan látható nézetet (ViewModel-t)
    [ObservableProperty]
    private ViewModelBase _currentPage;

    public MainViewModel()
    {
        // Alapértelmezett oldal a főmenü legyen
        _currentPage = new MainMenuViewModel();
    }

    // Parancsok az oldalak váltásához
    [RelayCommand]
    public void GoToCreateWorld() => CurrentPage = new CreateWorldViewModel();

    [RelayCommand]
    public void GoToGame() => CurrentPage = new GameViewModel();

    [RelayCommand]
    public void GoToMainMenu() => CurrentPage = new MainMenuViewModel();
}

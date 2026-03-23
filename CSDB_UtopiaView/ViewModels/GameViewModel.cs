using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSDB_UtopiaModel.Model;
using CSDB_UtopiaModel.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CSDB_UtopiaView.ViewModels;

public partial class GameViewModel : ViewModelBase
{
    // Mezők és tulajdonságok a diagram alapján
    private readonly Model _model;

    [ObservableProperty]
    private int _height;

    [ObservableProperty]
    private int _width;

    [ObservableProperty]
    private int _budget;

    [ObservableProperty]
    private int _currentMood;

    [ObservableProperty]
    private int _population;

    [ObservableProperty]
    private string _currentDateString = string.Empty;

    public Dictionary<Resource, int> DisplayStorage { get; } = new();

    public ObservableCollection<Cell> Cells { get; }

    // Építési panel láthatósága és a gombok listája
    [ObservableProperty] private bool _isBuildingPanelVisible;
    public ObservableCollection<Type> AvailableBuildables { get; } = new();

    // Tároljuk, hogy a felhasználó éppen mit választott ki építésre
    private Type? _selectedType;

    [ObservableProperty]
    private bool _isDemolishMode;

    // Események
    public event EventHandler? NewGame;
    public event EventHandler? GameOver;

    // Konstruktor
    public GameViewModel(int width, int height, Model model)
    {
        _width = width;
        _height = height;
        _model = model;
        Cells = new ObservableCollection<Cell>();

        // Feliratkozás a modell eseményeire
        _model.GameTicked += Model_GameTicked;
        _model.FieldsUpdated += Model_FieldsUpdated;
        _model.BudgetChanged += Model_BudgetChanged;
        _model.ResourceChanged += Model_ResourceChanged;
        _model.NewLog += Model_NewLog;
        _model.NewGame += Model_NewGame;
        _model.GameOver += Model_GameOver;
        _model.DateChanged += Model_DateChanged;

        Budget = _model.GetBudget();
        CurrentMood = _model.GetMood();
        Population = _model.GetResourceCount(HumanResource.Instance());

        InitializeDisplayStorage();

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var cell = new Cell(i, j);
                // Lekérjük a modellből az adott mezőt és frissítjük a cellát
                var field = _model.GetField(i, j); // Feltételezve, hogy van ilyen metódusod
                cell.Update(field);
                Cells.Add(cell);
            }
        }
    }

    private void InitializeDisplayStorage()
    {
        // Listába gyűjtjük az összes Singleton erőforrást
        var allResources = new List<Resource>
    {
        HumanResource.Instance(),
        Wood.Instance(), IronOre.Instance(), Coal.Instance(), Oil.Instance(),
        Gold.Instance(), Diamond.Instance(),
        Plank.Instance(), Iron.Instance(), Gasoline.Instance(),
        Jewelry.Instance(), Paper.Instance(), Book.Instance()
    };

        // Feltöltjük a szótárat a Model-ből lekért aktuális darabszámokkal
        foreach (var res in allResources)
        {
            DisplayStorage[res] = _model.GetResourceCount(res);
        }

        // Jelezünk a UI-nak
        OnPropertyChanged(nameof(DisplayStorage));
    }

    // Parancsok (RelayCommands)
    [RelayCommand]
    public void SaveGame() { /* Mentés logika */ }

    [RelayCommand]
    public void LoadGame(string fileName) { /* Betöltés logika */ }

    [RelayCommand]
    public void IncreaseSpeed() { }

    [RelayCommand]
    public void DecreaseSpeed() { }

    [RelayCommand]
    public void SetSpeed(TimerSpeed speed) { }

    [RelayCommand]
    public void Resume() { }

    [RelayCommand]
    public void Pause() { }

    //Építés
    [RelayCommand]
    public void ListBuildableOtherBuildings()
    {
        // Ha már látszik a panel, bezárjuk és töröljük a kijelölést
        if (IsBuildingPanelVisible)
        {
            IsBuildingPanelVisible = false;
            _selectedType = null;
        }
        else
        {
            // Lekérjük a Modell-től az aktuálisan építhető dolgokat
            AvailableBuildables.Clear();
            var types = _model.ListBuildableOtherBuildings();
            foreach (var type in types)
            {
                AvailableBuildables.Add(type);
            }
            IsBuildingPanelVisible = true;
        }
    }

    [RelayCommand]
    public void SelectBuildable(Type selectedType)
    {
        // Eltároljuk a választott típust
        _selectedType = selectedType;
        IsDemolishMode = false;
        // Opcionális: a panelt nyitva hagyjuk, ha többet akarunk építeni egymás után
        IsBuildingPanelVisible = false; 
    }

    [RelayCommand]
    public void Demolish()
    {
        IsDemolishMode = !IsDemolishMode;

        // Ha bekapcsoljuk a bontást, ne legyen kiválasztott épület
        if (IsDemolishMode)
        {
            _selectedType = null;
            IsBuildingPanelVisible = false;
        }
    }

    [RelayCommand]
    public void ClickCell(Cell cell)
    {

        // BONTÁS LOGIKA
        if (IsDemolishMode)
        {
            _model.Demolish(new Coordinate(cell.X, cell.Y));
            return;
        }

        // ÉPÍTÉS LOGIKA
        if (_selectedType != null)
        {
            Field targetField = _model.GetField(cell.X, cell.Y);
            if (Activator.CreateInstance(_selectedType, targetField) is Buildable instance)
            {
                _model.Place(new Coordinate(cell.X, cell.Y), instance);
            }
        }
    }

    [RelayCommand]
    public void ClickMiniMap(Coordinate coords) { }

    // Listázó parancsok
    [RelayCommand]
    public void ListBuildableFactories() { }

    [RelayCommand]
    public void ListBuildableProducers() { }

    [RelayCommand]
    public void ListBuildableDecorations() { }

    [RelayCommand]
    public void ListBuildableRoads() { }

    [RelayCommand]
    public void ListBuyablePassengerVehicles() { }

    [RelayCommand]
    public void ListBuyableIndustrialVehicles() { }

    // Modell eseménykezelők kifejtése
    private void Model_GameTicked(object? sender, EventArgs e)
    {
        // Időalapú frissítések a UI-on, ha szükséges
        // A konkrét dátumváltozást a Model_DateChanged kezeli
    }

    private void Model_FieldsUpdated(object? sender, FieldEventArgs e)
    {
        // Amikor a Model jelzi, hogy bizonyos mezők megváltoztak (pl. építés történt)
        foreach (var field in e.Fields)
        {
            // Megkeressük a megfelelő Cell objektumot az ObservableCollection-ben
            var cell = Cells.FirstOrDefault(c => c.X == field.Coordinates.X && c.Y == field.Coordinates.Y);
            if (cell != null)
            {
                // Frissítjük a Cell nézetmodelljét a Field adatai alapján (pl. kép lecserélése)
                cell.Update(field); 
            }
        }
    }

    private void Model_BudgetChanged(object? sender, EventArgs e)
    {
         Budget = _model.GetBudget();
    }

    private void Model_ResourceChanged(object? sender, ResourceChangedEventArgs e)
    {
        // Frissítjük a belső szótárat
        DisplayStorage[e.Resource] = e.NewValue;

        if (e.Resource is HumanResource)
        {
            Population = e.NewValue;
        }

        // Jelezzük a UI-nak, hogy a szótár tartalma megváltozott
        OnPropertyChanged(nameof(DisplayStorage));
    }

    private void Model_MoodChanged(object? sender, MoodChangedEventArgs e)
    {
        CurrentMood = e.Mood;
    }

    private void Model_NewLog(object? sender, LogEventArgs e)
    {
        // Új üzenet érkezésekor (pl. "Nincs elég pénz") hozzáadjuk egy napló-listához
        // GameLogs.Add(e.Message);
    }

    private void Model_NewGame(object? sender, EventArgs e)
    {
        // Értesítjük a View-t, hogy új játék kezdődött (pl. ablak alaphelyzetbe állítása)
        NewGame?.Invoke(this, EventArgs.Empty);
    }

    private void Model_GameOver(object? sender, EventArgs e)
    {
        // Értesítjük a View-t a játék végéről (pl. Game Over ablak megjelenítése)
        GameOver?.Invoke(this, EventArgs.Empty);
    }

    private void Model_DateChanged(object? sender, EventArgs e)
    {
        // Frissítjük a kijelzett dátumot a UI-on
        // CurrentDate = _model.GetCurrentDate();
    }

    // Egyéb metódusok
    public Field GetField()
    {
        throw new NotImplementedException();
    }
}
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSDB_UtopiaModel.Model;
using CSDB_UtopiaModel.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

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

    public Dictionary<IResource, int> DisplayStorage { get; } = new();

    public ObservableCollection<KeyValuePair<IResource, int>> StorageList { get; } = new();

    public ObservableCollection<Cell> Cells { get; }

    // Építési panel láthatósága és a gombok listája
    [ObservableProperty] private bool _isBuildingPanelVisible;
    public ObservableCollection<BuildableInfo> AvailableBuildables { get; } = new();

    // Tároljuk, hogy a felhasználó éppen mit választott ki építésre
    private Type? _selectedType;

    [ObservableProperty]
    private bool _isDemolishMode;

    [ObservableProperty]
    private string _currentTime = "00:00:00";

    [ObservableProperty]
    private bool _isPaused = true;

    [ObservableProperty]
    private bool _isGameOver;

    [ObservableProperty]
    private int _speedLevel = 1; // Alapértelmezett: Normal (1)

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
        _model.MoodChanged += Model_MoodChanged;

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
        var allResources = new List<IResource>
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

        RefreshStorageList();
    }

    private void RefreshStorageList()
    {
        StorageList.Clear();
        foreach (var entry in DisplayStorage)
        {
            StorageList.Add(entry);
        }
    }

    // Parancsok (RelayCommands)
    [RelayCommand]
    public void SaveGame() { /* Mentés logika */ }

    [RelayCommand]
    public void LoadGame(string fileName) { /* Betöltés logika */ }

    [RelayCommand]
    public void ExitGame()
    {
        Environment.Exit(0);
    }

    [RelayCommand]
    public void RestartGame()
    {
        IsGameOver = false;
        IsPaused = false;

        // Ténylegesen újrainicializáljuk a modellt
        _model.Reset(Width, Height);

        // Frissítjük a helyi property-ket
        Budget = _model.GetBudget();
        CurrentMood = _model.GetMood();

        // Újratöltjük a nyersanyagokat a kijelzőn
        InitializeDisplayStorage();
    }

    [RelayCommand]
    public void IncreaseSpeed()
    {
        try
        {
            _model.SpeedUp();
            // A TimerSpeed enum alapján: Normal=0, Fast=1, SpeedOfLight=2
            // Hogy 1-től induljon a skála:
            SpeedLevel = (int)TimeControl.Instance().Speed + 1;
        }
        catch (Exception) { /* Max sebesség */ }
    }

    [RelayCommand]
    public void DecreaseSpeed()
    {
        try
        {
            _model.SlowDown();
            SpeedLevel = (int)TimeControl.Instance().Speed + 1;
        }
        catch (Exception) { /* Min sebesség */ }
    }

    [RelayCommand]
    public void SetSpeed(TimerSpeed speed) { }

    [RelayCommand]
    public void Resume()
    {
        _model.TogglePause();
        IsPaused = _model.IsPaused();
    }

    [RelayCommand]
    public void Pause() { }

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
        try
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
                // Út építése
                if (_selectedType.IsAssignableTo(typeof(Road)))
                {
                    // Itt hívjuk meg a Modell metódusát, ami lekezeli a 
                    // tájolást és a hurok-ellenőrzést
                    _model.PlaceRoad(new Coordinate(cell.X, cell.Y));
                    return; // Ne menjen tovább az építés
                }

                Field targetField = _model.GetField(cell.X, cell.Y);
                Buildable? instance = null;

                // Ellenőrizzük, hogy ResourceExtractor-ról van-e szó
                //TODO többi gyár
                if (_selectedType.IsAssignableTo(typeof(ResourceExtractor)))
                {
                    // Itt meg kell adni egy alapértelmezett yield értéket (pl. 10)
                    instance = (Buildable?)Activator.CreateInstance(_selectedType, targetField, 10);
                }
                else if (_selectedType.IsAssignableTo(typeof(Factory)))
                {
                    instance = (Buildable?)Activator.CreateInstance(_selectedType, targetField, 30);
                }       

                else
                {
                    // Sima 1 paraméteres konstruktor (pl. lakóház, út)
                    instance = (Buildable?)Activator.CreateInstance(_selectedType, targetField);
                }

                if (instance != null)
                {
                    _model.Place(new Coordinate(cell.X, cell.Y), instance);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            // Itt kapjuk el a "Can't place it here" és hasonló hibákat
            // Ahelyett, hogy összeomlana, pl. kiírhatjuk a Debug konzolra vagy egy Log listába
            System.Diagnostics.Debug.WriteLine($"Építési hiba: {ex.Message}");

            // Ha van Log
            // CurrentLogMessage = ex.Message; 
        }
        catch (Exception ex)
        {
            // Minden más váratlan hiba elkapása
            System.Diagnostics.Debug.WriteLine($"Váratlan hiba: {ex.Message}");
        }
    }

    [RelayCommand]
    public void ClickMiniMap(Coordinate coords) { }

    // Listázó parancsok
    [RelayCommand]
    public void ListBuildableFactories() {
        UpdateAvailableBuildables(_model.ListBuildableFactories());
    }

    [RelayCommand]
    public void ListBuildableResidential()
    {
        UpdateAvailableBuildables(_model.ListBuildableResidential());
    }

    [RelayCommand]
    public void ListBuildableResourceExtractors() {
        UpdateAvailableBuildables(_model.ListBuildableResourceExtractors());
    }

    [RelayCommand]
    public void ListBuildableDecorations() {
        UpdateAvailableBuildables(_model.ListBuildableDecorations());
    }

    [RelayCommand]
    public void ListBuildableRoads() {
        UpdateAvailableBuildables(_model.ListBuildableRoads());
    }

    [RelayCommand]
    public void ListBuyablePassengerVehicles() {
        // Lekérjük az utas-szállítókat
        var passengers = _model.ListBuyablePassengerVehicles();
        // Lekérjük az ipari szállítókat
        var industrial = _model.ListBuyableIndustrialVehicles();

        // Összefűzzük a kettőt és frissítjük a panelt
        UpdateAvailableBuildables(passengers.Concat(industrial).ToList());
    }

    [RelayCommand]
    public void ListBuyableIndustrialVehicles()
    {
        UpdateAvailableBuildables(_model.ListBuyableIndustrialVehicles());
    }

    [RelayCommand]
    public void ListBuildableOtherBuildings()
    {
        UpdateAvailableBuildables(_model.ListBuildableOtherBuildings());
    }

    [RelayCommand]
    public void CloseBuildingPanel()
    {
        IsBuildingPanelVisible = false;
        _selectedType = null; // Töröljük a kijelölést is, ha volt
        AvailableBuildables.Clear();
    }

    private void UpdateAvailableBuildables(List<Type> types)
    {
        // Panel bezárása, ha ugyanazt a kategóriát kattintjuk
        if (IsBuildingPanelVisible && AvailableBuildables.Count > 0 &&
            types.Count > 0 && AvailableBuildables[0].Type == types[0])
        {
            IsBuildingPanelVisible = false;
            AvailableBuildables.Clear();
            return;
        }

        AvailableBuildables.Clear();

        foreach (var type in types)
        {
            try
            {
                if (type.IsAbstract || type.IsInterface) continue;

                string displayName = type.Name;
                if (displayName.Contains('`'))
                {
                    displayName = displayName.Substring(0, displayName.IndexOf('`'));
                }
                BuildableInfo info = new() { Name = displayName, Type = type };

                // JÁRMŰVEK (Biztonsági fix értékekkel, hogy ne szálljon el a throw miatt)
                if (type.IsAssignableTo(typeof(IVehicle)))
                {
                    info.IsVehicle = true;
                    info.Speed = 60;
                    info.Maintenance = 200;
                    info.Capacity = 40;
                    // Itt NEM hívunk Activator-t, amíg a modellben throw new NotImplementedException() van!
                }
                // ÉPÜLETEK ÉS UTAK
                else
                {
                    Buildable? bDummy = null;

                    if (type.IsAssignableTo(typeof(ResourceExtractor)))
                        bDummy = (Buildable?)Activator.CreateInstance(type, null, 10);
                    else if (type.IsAssignableTo(typeof(Factory)))
                        bDummy = (Buildable?)Activator.CreateInstance(type, null, 30);
                    else if (type.IsAssignableTo(typeof(Road)))
                    {
                        // PRÓBÁLKOZÁS SORRENDJE FONTOS
                        try
                        {
                            // 1. Általános út (3 paraméter: Field, int, IDirection)
                            // A 'null' helyett adjunk át egy érvényes irány típust, ha a híd kéri
                            bDummy = (Buildable?)Activator.CreateInstance(type, null, 50, null);
                        }
                        catch
                        {
                            try
                            {
                                // 2. Hidak (2 paraméter: Field, IDirection)
                                // Itt is a null IDirection lehet a gond, ha a híd konstruktora használja
                                bDummy = (Buildable?)Activator.CreateInstance(type, null, null);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Road hiba ({type.Name}): {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        // Házak, dekorok (1 paraméter)
                        bDummy = (Buildable?)Activator.CreateInstance(type, (Field?)null);
                    }

                    if (bDummy != null)
                    {
                        info.PlacementCost = bDummy.placementCost;
                        if (bDummy is Decoration decor && decor.costResource.resource != null)
                            info.ResourceRequirement = $"{decor.costResource.cost} {decor.costResource.resource.GetType().Name}";
                    }
                    else
                    {
                        // Ha nem sikerült példányosítani, adjunk meg alapértelmezett árat, 
                        // hogy legalább a gomb megjelenjen
                        info.PlacementCost = 0;
                    }
                }

                AvailableBuildables.Add(info);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Hiba a(z) {type.Name} feldolgozásakor: {ex.Message}");
            }
        }

        IsDemolishMode = false;
        IsBuildingPanelVisible = AvailableBuildables.Count > 0;
    }

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

        RefreshStorageList();
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
        IsPaused = true; // Állítsuk meg az időt
        IsGameOver = true; // Ez fogja aktiválni az ablakot
        GameOver?.Invoke(this, EventArgs.Empty);
    }

    private void Model_DateChanged(object? sender, EventArgs e)
    {
        // Minden tick-nél frissül a string
        CurrentTime = _model.GetFormattedTime();
    }

    // Egyéb metódusok
    public Field GetField()
    {
        throw new NotImplementedException();
    }
}

public class BuildableInfo
{
    public string Name { get; set; } = string.Empty;
    public Type Type { get; set; } = null!;
    public int PlacementCost { get; set; }
    public string ResourceRequirement { get; set; } = string.Empty;
    public bool HasResourceRequirement => !string.IsNullOrEmpty(ResourceRequirement);

    // Jármű specifikus adatok
    public bool IsVehicle { get; set; }
    public int Speed { get; set; }
    public int Maintenance { get; set; }
    public int Capacity { get; set; }
}
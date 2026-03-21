using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CSDB_UtopiaView.Views;
public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CSDB_UtopiaView.Views;

public partial class LoadingView : UserControl
{
    public LoadingView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
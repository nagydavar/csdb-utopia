using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CSDB_UtopiaView.Views;
public partial class CreateWorldView : UserControl
{
    public CreateWorldView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
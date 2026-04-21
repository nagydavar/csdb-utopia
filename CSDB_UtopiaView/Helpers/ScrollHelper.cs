using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace CSDB_UtopiaView.Helpers
{
    public static class ScrollHelper
    {
        public static readonly AttachedProperty<Vector> ScrollOffsetProperty =
        AvaloniaProperty.RegisterAttached<ScrollViewer, Vector>("ScrollOffset", typeof(ScrollHelper));

        static ScrollHelper()
        {
            ScrollOffsetProperty.Changed.Subscribe(e =>
            {
                // Az 'e.Sender' a ScrollViewer, az 'e.NewValue.Value' az új Vector
                if (e.Sender is ScrollViewer scrollViewer && e.NewValue.Value is Vector offset)
                {
                    // Közvetlenül az Offset tulajdonságot állítjuk
                    scrollViewer.Offset = offset;

                    // Biztonsági mentés: néha az Avalonia igényli a Layout frissítést
                    scrollViewer.InvalidateMeasure();
                }
            });
        }

        public static void SetScrollOffset(AvaloniaObject element, Vector value) => element.SetValue(ScrollOffsetProperty, value);
        public static Vector GetScrollOffset(AvaloniaObject element) => (Vector)element.GetValue(ScrollOffsetProperty)!;
    }
}

using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDB_UtopiaView.Converters
{
    public class CellToMinimapColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string? path = value as string;

            // Ha üres a string, akkor valószínűleg alap föld mező
            if (string.IsNullOrEmpty(path)) return Brushes.LimeGreen;

            // Fontos: Kis/Nagybetű érzékenység elkerülése
            string p = path.ToLower();

            if (p.Contains("water")) return Brushes.RoyalBlue;
            if (p.Contains("mountain")) return Brushes.DarkGray;
            if (p.Contains("roads")) return Brushes.LightGray;
            if (p.Contains("factories")) return Brushes.OrangeRed;
            if (p.Contains("resourceextractors")) return Brushes.OrangeRed;
            if (p.Contains("decorations")) return Brushes.Gold;
            if (p.Contains("residentialbuilding")) return Brushes.ForestGreen;
            if (p.Contains("otherbuildings")) return Brushes.MediumPurple;

            return Brushes.LimeGreen;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}

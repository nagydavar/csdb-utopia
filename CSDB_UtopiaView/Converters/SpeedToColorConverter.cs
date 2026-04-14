using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace CSDB_UtopiaView.Converters
{
    public class SpeedToColorConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            // values[0]: aktuális SpeedLevel (int)
            // values[1]: a négyzet sorszáma (int - amit a parameterben adunk át)
            if (values.Count >= 1 && values[0] is int currentLevel && parameter is string p && int.TryParse(p, out int squareIndex))
            {
                return currentLevel >= squareIndex ? Brushes.Red : Brushes.Gray;
            }
            return Brushes.Gray;
        }
    }
}

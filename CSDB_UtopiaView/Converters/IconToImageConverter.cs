using Avalonia.Data.Converters;
using CSDB_UtopiaView.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDB_UtopiaView.Converters
{
    public class IconToImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                // Itt az ImageLoader.Get-nek egy Bitmap-et vagy IImage-et KELL visszaadnia
                return ImageLoader.Get(path);
            }
            return null;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Windows.Data;

namespace PsControls.Windows.Data
{
    /// <summary>
    /// Return null if the string is empty. Used for Tooltips so they don't show in empty strings.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class EmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? null : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

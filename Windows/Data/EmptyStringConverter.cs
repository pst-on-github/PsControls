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
        /// <summary>
        /// Converts a string to null if it is empty.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="targetType">Mandatory string.</param>
        /// <param name="parameter">Parameter is silently ignored.</param>
        /// <param name="culture">Culture is silently ignored.</param>
        /// <returns>Null if the string <paramref name="value"/> is empty. Otherwise the string is returned unchanged.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? null : value;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

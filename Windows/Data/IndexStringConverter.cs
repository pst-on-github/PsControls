using System;
using System.Windows.Data;

namespace PsControls.Windows.Data
{
    /// <summary>
    /// Binds to an integer or bool and uses their value as index to retreave
    /// a string from an array passed as parameter.
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(bool), typeof(string))]
    public class IndexStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int index = value switch
            {
                bool boolValue => System.Convert.ToInt32(boolValue),
                int intValue => intValue,
                null => throw new ArgumentNullException(nameof(value)),
                _ => throw new ArgumentException("Type not supported: {value}", value.GetType().ToString()),
            };

            if (parameter is string[] titles)
            {
                if (titles.Length == 0)
                {
                    throw new ArgumentException("string[] cannot be empty", nameof(parameter));
                }

                index = Math.Min(index, titles.Length - 1);
                return titles[index];
            }

            throw new ArgumentException("{name} is not a string array.", nameof(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

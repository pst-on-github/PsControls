using System;
using System.Windows.Data;

namespace PsControls.Windows.Data
{
    /// <summary>
    /// Binds to an integer or bool and uses their value as index to retrieve
    /// a string from an array passed as parameter.
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(bool), typeof(string))]
    public class IndexStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a bool or integer to an index to a table of strings entry.
        /// The table must be provided as <paramref name="parameter"/>.
        /// </summary>
        /// <param name="value">The value to be used as an index.</param>
        /// <param name="targetType">Target type is silently ignored.</param>
        /// <param name="parameter">The non empty string array to be indexed.</param>
        /// <param name="culture">Culture is silently ignored.</param>
        /// <returns>The string table entry indexed by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the type of <paramref name="value"/> is neither bool nor int.
        /// Or the string table given by <paramref name="parameter"/> is not of type string[].</exception>
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

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

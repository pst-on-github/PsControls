using System;
using System.Windows;
using System.Windows.Data;

namespace PsControls.Windows.Data
{
    /// <summary>
    /// Converts value to a visibility. Convert back is not implemented.
    /// </summary>
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts value to indicate visibility.
        /// </summary>
        /// <param name="value">The value indicating visibility.</param>
        /// <param name="targetType">The target type expected. Supposed to be <see cref="Visibility"/>.</param>
        /// <param name="parameter">Parameter controlling Hidden or Collapsed in case on invisibility.</param>
        /// <param name="culture">Culture to be used for conversions.</param>
        /// <returns>
        /// Returns Visible if the value indicates true. If <paramref name="value"/> indicates false
        /// returns Hidden or Collapsed depending on <paramref name="parameter"/>. By default Hidden is returned.
        /// If <paramref name="parameter"/> is of type string only Hidden or Collapsed is allowed.
        /// </returns>
        /// <remarks>A string that is null or empty converts to not visible.</remarks>
        /// <exception cref="ArgumentException">Thrown if parameter is string and it is not Hidden or Collapsed or
        /// targetType is not <see cref="Visibility"/>.</exception>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
            {
                throw new ArgumentException("Must be of type Visibility", nameof(targetType));
            }

            Visibility hiddenOrCollapsed = Visibility.Hidden;

            if (parameter is string stringParameter)
            {
                Visibility?[] allowed = { Visibility.Collapsed, Visibility.Hidden };

                Visibility? found = Array.Find(allowed, a => a.ToString() == stringParameter);
                if (found.HasValue)
                {
                    hiddenOrCollapsed = found.Value;
                }
                else
                {
                    throw new ArgumentException($"Allowed values are: {string.Join(',', allowed)}", nameof(parameter));
                }
            }

            bool isVisible = true;

            if (value is string stringValue)
            {
                isVisible = !string.IsNullOrEmpty(stringValue);
            }
            else
            {
                isVisible = System.Convert.ToBoolean(value, culture);
            }

            return isVisible ? Visibility.Visible : hiddenOrCollapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Convert back is not implemented
            throw new NotImplementedException();
        }
    }
}

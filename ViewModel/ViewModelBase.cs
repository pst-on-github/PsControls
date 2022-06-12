using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PsControls.ViewModel
{
    /// <summary>
    /// Provides basic feature for view models.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when any event changes its value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Provides the plural 's' depending on <paramref name="count"/>.
        /// </summary>
        /// <param name="count">Private the count for whether to return 's' or not.</param>
        /// <returns>"s" if <paramref name="count"/> is not one.</returns>
        public static string Ps(int count)
        {
            return count != 1 ? "s" : string.Empty;
        }

        /// <summary>
        /// Called when any of the properties changes its value.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

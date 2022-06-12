using System;
using System.Windows.Input;

namespace PsControls.Windows.Input
{
    /// <summary>
    /// Provides data for the <see cref="DelegateCommand.CommandCanExecute"/> events.
    /// </summary>
    public class CanExecuteDelegateEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="DelegateCommand"/>
        /// associated with this event can be executed.
        /// </summary>
        public bool CanExecute { get; set; }
    }
}

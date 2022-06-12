using System;
using System.Windows.Input;

namespace PsControls.Windows.Input
{
    /// <summary>
    /// Provides data for the <see cref="DelegateCommand.CommandExecuted"/> events.
    /// </summary>
    public class ExecutedDelegateEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutedDelegateEventArgs"/> class.
        /// </summary>
        /// <param name="commandParameter">The command parameter.</param>
        public ExecutedDelegateEventArgs(object commandParameter = null)
        {
            Parameter = commandParameter;
        }

        /// <summary>
        /// Gets data parameter of the command.
        /// </summary>
        public object Parameter { get; }
    }
}

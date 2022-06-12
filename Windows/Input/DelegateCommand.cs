using System;
using System.Windows.Input;

namespace PsControls.Windows.Input
{
    /// <summary>
    /// DelegateCommand borrowed from
    /// http://www.wpftutorial.net/DelegateCommand.html.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        public DelegateCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to be called when the command executes.</param>
        /// <param name="canExecute">
        /// The predicate to be called to check whether
        /// the command can be executed.
        /// </param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when this command executes.
        /// </summary>
        public event EventHandler<ExecutedDelegateEventArgs> CommandExecuted;

        /// <summary>
        /// Occurs when this command initiates a check to determine whether the command can be executed.
        /// </summary>
        public event EventHandler<CanExecuteDelegateEventArgs> CommandCanExecute;

        // ================
        // ICommand Members
        // ================

        /// <inheritdoc cref="ICommand.CanExecuteChanged" />
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc cref="ICommand.CanExecute" />
        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute.Invoke(parameter);
            }
            else if (CommandCanExecute != null)
            {
                CanExecuteDelegateEventArgs args = new () { CanExecute = false };

                CommandCanExecute.Invoke(this, args);

                return args.CanExecute;
            }

            return true;
        }

        /// <inheritdoc cref="ICommand.Execute" />
        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute?.Invoke(parameter);
            }
            else if (CommandExecuted != null)
            {
                CommandExecuted.Invoke(this, new ExecutedDelegateEventArgs(parameter));
            }
        }

        /// <summary>
        /// Raise the <see cref="CanExecuteChanged"/>  event.
        /// </summary>
        public virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

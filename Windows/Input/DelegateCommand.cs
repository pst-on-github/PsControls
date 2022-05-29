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
        private readonly Action<object, DelegateEventArgs> _executeEvent;
        private readonly Action<object> _execute;

        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<object, DelegateEventArgs> execute, Predicate<object> canExecute)
        {
            _executeEvent = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members

        /// <inheritdoc cref="ICommand.CanExecuteChanged" />
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc cref="ICommand.CanExecute" />
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute.Invoke(parameter);
        }

        /// <inheritdoc cref="ICommand.Execute" />
        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
            _executeEvent?.Invoke(this, new DelegateEventArgs(parameter));
        }

        #endregion

        /// <summary>
        /// Raise the <see cref="CanExecuteChanged"/>  event.
        /// </summary>
        public virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

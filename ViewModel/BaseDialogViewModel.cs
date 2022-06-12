using System.Windows.Input;

namespace PsControls.ViewModel
{
    /// <summary>
    /// Provides basic view model features for dialogs.
    /// </summary>
    public class BaseDialogViewModel : ViewModelBase
    {
        private object _content;
        private object _contentStringFormat;
        private bool _isOpen;
        private string _title;
        private bool _showCancelButton = true;

        /// <summary>
        /// Gets or sets the command to invoke when the dialog is canceled.
        /// </summary>
        public ICommand CancelCommand { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this dialog is open (shown).
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (value != _isOpen)
                {
                    _isOpen = value;
                    OnIsOpenChanged(_isOpen);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the title text of this dialog.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the content of a this dialog.
        /// </summary>
        public object Content
        {
            get => _content;
            set
            {
                if (value != _content)
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a composite string that specifies how to format the
        /// <see cref="Content"/> property if it is displayed as a string.
        /// </summary>
        public object ContentStringFormat
        {
            get => _contentStringFormat;
            set
            {
                if (value != _contentStringFormat)
                {
                    _contentStringFormat = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a 'Cancel' button shall be displayed.
        /// </summary>
        public bool ShowCancelButton
        {
            get => _showCancelButton;

            set
            {
                if (_showCancelButton != value)
                {
                    _showCancelButton = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Displays the dialog. See also <seealso cref="IsOpen" /> property.
        /// </summary>
        public virtual void Show()
        {
            DialogService.Attach(this);
            IsOpen = true;
        }

        /// <summary>
        /// Called when the <see cref="IsOpen" /> property changed.
        /// </summary>
        /// <param name="isOpen">A value indicating whether the dialog is now open.</param>
        protected virtual void OnIsOpenChanged(bool isOpen)
        {
            // Provided to be overwritten
        }
    }
}

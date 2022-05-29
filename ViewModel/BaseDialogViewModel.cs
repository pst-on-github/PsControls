using PsControls.Windows.Input;

namespace PsControls.ViewModel
{
    public class BaseDialogViewModel : ViewModelBase
    {
        private object _content;
        private object _contentStringFormat;
        private bool _isOpen;
        private string _title;

        public DelegateCommand CancelCommand { get; protected set; }

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

        public virtual void Show()
        {
            DialogService.Attach(this);
            IsOpen = true;
        }

        protected virtual void OnIsOpenChanged(bool isOpen)
        {
            // Provided to be overwritten
        }
    }
}

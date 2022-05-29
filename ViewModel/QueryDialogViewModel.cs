using System.Threading.Tasks;
using PsControls.Windows.Input;

namespace PsControls.ViewModel
{
    public class QueryDialogViewModel : BaseDialogViewModel
    {
#pragma warning disable S1450
        private bool _answer;
#pragma warning restore S1450

        private TaskCompletionSource _completionSource;

        public QueryDialogViewModel()
        {
            QueryOkCommand = new DelegateCommand((object _) =>
            {
                _answer = true;
                _completionSource.TrySetResult();
            });
            CancelCommand = new DelegateCommand((object _) => _completionSource.TrySetResult());
        }

        public DelegateCommand QueryOkCommand { get; }

        public async Task<bool> ShowDialogAsync()
        {
            _answer = false;
            _completionSource = new();

            Show();
            await _completionSource.Task.ConfigureAwait(true);

            IsOpen = false;
            return _answer;
        }

        protected override void OnIsOpenChanged(bool isOpen)
        {
            if (!isOpen)
            {
                _completionSource.TrySetResult();
            }
        }
    }
}

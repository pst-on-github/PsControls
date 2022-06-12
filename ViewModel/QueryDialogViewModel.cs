using System.Threading.Tasks;
using PsControls.Windows.Input;

namespace PsControls.ViewModel
{
    /// <summary>
    /// Provides a view model that can be used ti query user decisions.
    /// Similar to a message box.
    /// </summary>
    public class QueryDialogViewModel : BaseDialogViewModel
    {
#pragma warning disable S1450
        private bool _answer;
#pragma warning restore S1450

        private TaskCompletionSource _completionSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryDialogViewModel"/> class.
        /// </summary>
        public QueryDialogViewModel()
        {
            QueryOkCommand = new DelegateCommand((object _) =>
            {
                _answer = true;
                _completionSource.TrySetResult();
            });
            CancelCommand = new DelegateCommand((object _) => _completionSource.TrySetResult());
        }

        /// <summary>
        /// Gets the command to invoke when the OK button is clicked.
        /// </summary>
        public DelegateCommand QueryOkCommand { get; }

        /// <summary>
        /// Opens the dialog asynchronously. So it can be awaited.
        /// </summary>
        /// <returns>A value indicating whether the dialog was confirmed (i.e. the OK button was clicked). </returns>
        public async Task<bool> ShowDialogAsync()
        {
            _answer = false;
            _completionSource = new ();

            Show();
            await _completionSource.Task.ConfigureAwait(true);

            IsOpen = false;
            return _answer;
        }

        /// <inheritdoc/>
        protected override void OnIsOpenChanged(bool isOpen)
        {
            if (!isOpen)
            {
                _completionSource.TrySetResult();
            }
        }
    }
}

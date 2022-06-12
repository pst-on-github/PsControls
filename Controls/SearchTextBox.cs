using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PsControls.Controls
{
    /// <summary>
    /// Defines possible operation modes of the <see cref="SearchTextBox"/>.
    /// </summary>
    public enum SearchMode
    {
        /// <summary>
        /// The search command is executed immediately.
        /// </summary>
        Instant,

        /// <summary>
        /// The search command is delayed.
        /// </summary>
        Delayed,
    }

    /// <summary>
    /// Represents a control similar to <see cref="TextBox"/>. Additionally it provides
    /// a background label text, a clear button and a delayed search event.
    /// </summary>
    public class SearchTextBox : TextBox
    {
        /// <summary>
        /// Identifies the <see cref="HasText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasTextProperty;

        /// <summary>
        /// Identifies the <see cref="IsLeftMouseButtonDown"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLeftMouseButtonDownProperty;

        /// <summary>
        /// Identifies the <see cref="LabelTextColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextColorProperty =
            DependencyProperty.Register(
                "LabelTextColor",
                typeof(Brush),
                typeof(SearchTextBox));

        /// <summary>
        /// Identifies the <see cref="LabelText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextProperty =
                DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(SearchTextBox));

        /// <summary>
        /// Identifies the <see cref="SearchMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchModeProperty =
            DependencyProperty.Register(
                "SearchMode",
                typeof(SearchMode),
                typeof(SearchTextBox),
                new PropertyMetadata(SearchMode.Instant));

        /// <summary>
        /// Identifies the <see cref="SearchCommandDelay"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchCommandDelayProperty =
            DependencyProperty.Register(
            "SearchCommandDelay",
            typeof(TimeSpan),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(
                TimeSpan.FromMilliseconds(250),
                new PropertyChangedCallback(SearchCommandDelayChanged)));

        /// <summary>
        /// Identifies the <see cref="SearchCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(
                "SearchCommand",
                typeof(ICommand),
                typeof(SearchTextBox));

        /// <summary>
        /// Identifies the <see cref="SearchCommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchCommandParameterProperty =
            DependencyProperty.Register(
                "SearchCommandParameter",
                typeof(object),
                typeof(SearchTextBox));

        private static readonly DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "HasText",
                typeof(bool),
                typeof(SearchTextBox),
                new PropertyMetadata());

        private static readonly DependencyPropertyKey IsLeftMouseButtonDownPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "IsLeftMouseButtonDown",
                typeof(bool),
                typeof(SearchTextBox),
                new PropertyMetadata());

        private readonly DispatcherTimer _searchCommandDelayTimer;

        #pragma warning disable S3963

        static SearchTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SearchTextBox),
                new FrameworkPropertyMetadata(typeof(SearchTextBox)));

            HasTextProperty = HasTextPropertyKey.DependencyProperty;
            IsLeftMouseButtonDownProperty = IsLeftMouseButtonDownPropertyKey.DependencyProperty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchTextBox"/> class.
        /// </summary>
        public SearchTextBox()
        {
            _searchCommandDelayTimer = new DispatcherTimer
            {
                Interval = SearchCommandDelay
            };

            _searchCommandDelayTimer.Tick += SearchEventDelayTimer_Tick;
        }

        /// <summary>
        /// Gets a value indicating whether the text property is not empty.
        /// </summary>
        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the left mouse button is down on this control.
        /// </summary>
        public bool IsLeftMouseButtonDown
        {
            get { return (bool)GetValue(IsLeftMouseButtonDownProperty); }
            private set { SetValue(IsLeftMouseButtonDownPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the label text shown as background of the text input area.
        /// </summary>
        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label text color as background of the text input area.
        /// </summary>
        public Brush LabelTextColor
        {
            get { return (Brush)GetValue(LabelTextColorProperty); }
            set { SetValue(LabelTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the mode or operation. See also <seealso cref="Controls.SearchMode"/>.
        /// </summary>
        public SearchMode SearchMode
        {
            get { return (SearchMode)GetValue(SearchModeProperty); }
            set { SetValue(SearchModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to invoke when the search event occurs. See also <seealso cref="SearchMode"/>.
        /// </summary>
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the time span before the search event/command occurs after the last character input.
        /// </summary>
        public TimeSpan SearchCommandDelay
        {
            get { return (TimeSpan)GetValue(SearchCommandDelayProperty); }
            set { SetValue(SearchCommandDelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="SearchCommand"/> property.
        /// </summary>
        public object SearchCommandParameter
        {
            get { return GetValue(SearchCommandParameterProperty); }
            set { SetValue(SearchCommandParameterProperty, value); }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_SearchIconBorder") is Border iconBorder)
            {
                iconBorder.MouseLeftButtonDown += IconBorder_LeftMouseButtonDown;
                iconBorder.MouseLeftButtonUp += IconBorder_LeftMouseButtonUp;
                iconBorder.MouseLeave += IconBorder_MouseLeave;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e">Provides data about the event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool handled = false;

            if (e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                if (e.Key == Key.Escape && SearchMode == SearchMode.Instant)
                {
                    Clear();
                    handled = true;
                }
                else if ((e.Key == Key.Return || e.Key == Key.Enter) && SearchMode == SearchMode.Delayed)
                {
                    OnSearch();
                    handled = true;
                }
            }

            if (!handled)
            {
                base.OnKeyDown(e);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e">Provides data about the event.</param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            HasText = Text.Length != 0;

            if (SearchMode == SearchMode.Instant)
            {
                _searchCommandDelayTimer.Stop();
                _searchCommandDelayTimer.Start();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected virtual void OnSearch()
        {
            if (SearchCommand?.CanExecute(SearchCommandParameter) == true)
            {
                SearchCommand?.Execute(SearchCommandParameter);
            }
        }

        private static void SearchCommandDelayChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs e)
        {
            if (dObj is SearchTextBox searchTextBox)
            {
                searchTextBox._searchCommandDelayTimer.Interval = ((Duration)e.NewValue).TimeSpan;
                searchTextBox._searchCommandDelayTimer.Stop();
            }
        }

        private void IconBorder_MouseLeave(object obj, MouseEventArgs e)
        {
            IsLeftMouseButtonDown = false;
        }

        private void IconBorder_LeftMouseButtonDown(object obj, MouseButtonEventArgs e)
        {
            IsLeftMouseButtonDown = true;
        }

        private void IconBorder_LeftMouseButtonUp(object obj, MouseButtonEventArgs e)
        {
            if (!IsLeftMouseButtonDown)
            {
                return;
            }

            if (HasText && SearchMode == SearchMode.Instant)
            {
                Clear();
            }

            if (HasText && SearchMode == SearchMode.Delayed)
            {
                OnSearch();
            }

            IsLeftMouseButtonDown = false;
        }

        private void SearchEventDelayTimer_Tick(object o, EventArgs e)
        {
            _searchCommandDelayTimer.Stop();
            OnSearch();
        }
    }
}

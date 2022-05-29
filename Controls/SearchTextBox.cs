using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PsControls.Controls
{
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

    public class SearchTextBox : TextBox
    {
        public static readonly DependencyProperty HasTextProperty;

        public static readonly DependencyProperty IsMouseLeftButtonDownProperty;

        public static readonly DependencyProperty LabelTextColorProperty =
            DependencyProperty.Register(
                "LabelTextColor",
                typeof(Brush),
                typeof(SearchTextBox));

        public static readonly DependencyProperty LabelTextProperty =
                DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(SearchTextBox));

        public static readonly DependencyProperty SearchModeProperty =
            DependencyProperty.Register(
                "SearchMode",
                typeof(SearchMode),
                typeof(SearchTextBox),
                new PropertyMetadata(SearchMode.Instant));

        public static readonly DependencyProperty SearchCommandDelayProperty =
            DependencyProperty.Register(
            "SearchCommandDelay",
            typeof(Duration),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(
                new Duration(new TimeSpan(0, 0, 0, 0, 250)),
                new PropertyChangedCallback(SearchCommandDelayChanged)));

        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(
                "SearchCommand",
                typeof(ICommand),
                typeof(SearchTextBox));

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

        private static readonly DependencyPropertyKey IsMouseLeftButtonDownPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "IsMouseLeftButtonDown",
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
            IsMouseLeftButtonDownProperty = IsMouseLeftButtonDownPropertyKey.DependencyProperty;
        }

        public SearchTextBox()
        {
            _searchCommandDelayTimer = new DispatcherTimer
            {
                Interval = SearchCommandDelay.TimeSpan
            };

            _searchCommandDelayTimer.Tick += SearchEventDelayTimer_Tick;
        }

        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextPropertyKey, value); }
        }

        public bool IsMouseLeftButtonDown
        {
            get { return (bool)GetValue(IsMouseLeftButtonDownProperty); }
            private set { SetValue(IsMouseLeftButtonDownPropertyKey, value); }
        }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public Brush LabelTextColor
        {
            get { return (Brush)GetValue(LabelTextColorProperty); }
            set { SetValue(LabelTextColorProperty, value); }
        }

        public SearchMode SearchMode
        {
            get { return (SearchMode)GetValue(SearchModeProperty); }
            set { SetValue(SearchModeProperty, value); }
        }

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public Duration SearchCommandDelay
        {
            get { return (Duration)GetValue(SearchCommandDelayProperty); }
            set { SetValue(SearchCommandDelayProperty, value); }
        }

        public object SearchCommandParameter
        {
            get { return GetValue(SearchCommandParameterProperty); }
            set { SetValue(SearchCommandParameterProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_SearchIconBorder") is Border iconBorder)
            {
                iconBorder.MouseLeftButtonDown += IconBorder_MouseLeftButtonDown;
                iconBorder.MouseLeftButtonUp += IconBorder_MouseLeftButtonUp;
                iconBorder.MouseLeave += IconBorder_MouseLeave;
            }
        }

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
            IsMouseLeftButtonDown = false;
        }

        private void IconBorder_MouseLeftButtonDown(object obj, MouseButtonEventArgs e)
        {
            IsMouseLeftButtonDown = true;
        }

        private void IconBorder_MouseLeftButtonUp(object obj, MouseButtonEventArgs e)
        {
            if (!IsMouseLeftButtonDown)
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

            IsMouseLeftButtonDown = false;
        }

        private void SearchEventDelayTimer_Tick(object o, EventArgs e)
        {
            _searchCommandDelayTimer.Stop();
            OnSearch();
        }
    }
}

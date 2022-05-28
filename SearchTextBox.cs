using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PsControls
{
    public enum SearchMode
    {
        Instant,
        Delayed,
    }

    public class SearchTextBox : TextBox
    {
        public static DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        public static DependencyProperty IsMouseLeftButtonDownProperty =
            IsMouseLeftButtonDownPropertyKey.DependencyProperty;

        public static DependencyProperty LabelTextColorProperty =
            DependencyProperty.Register(
                "LabelTextColor",
                typeof(Brush),
                typeof(SearchTextBox));

        public static DependencyProperty LabelTextProperty =
                DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(SearchTextBox));

        public static DependencyProperty SearchModeProperty =
            DependencyProperty.Register(
                "SearchMode",
                typeof(SearchMode),
                typeof(SearchTextBox),
                new PropertyMetadata(SearchMode.Instant));

        private static DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "HasText",
                typeof(bool),
                typeof(SearchTextBox),
                new PropertyMetadata());

        private static DependencyPropertyKey IsMouseLeftButtonDownPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsMouseLeftButtonDown",
            typeof(bool),
            typeof(SearchTextBox),
            new PropertyMetadata());

        static SearchTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SearchTextBox),
                new FrameworkPropertyMetadata(typeof(SearchTextBox)));
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_SearchIconBorder") is Border iconBorder)
            {
                iconBorder.MouseLeftButtonDown += new MouseButtonEventHandler(IconBorder_MouseLeftButtonDown);
                iconBorder.MouseLeftButtonUp += new MouseButtonEventHandler(IconBorder_MouseLeftButtonUp);
                iconBorder.MouseLeave += new MouseEventHandler(IconBorder_MouseLeave);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape && SearchMode == SearchMode.Instant)
            {
                this.Text = "";
            }
            else if ((e.Key == Key.Return || e.Key == Key.Enter) && SearchMode == SearchMode.Delayed)
            {
                // TODO RaiseSearchEvent();
            }
            else
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
                searchEventDelayTimer.Stop();
                searchEventDelayTimer.Start();
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
            if (!IsMouseLeftButtonDown) return;

            if (HasText && SearchMode == SearchMode.Instant)
            {
                this.Text = "";
            }
            if (HasText && SearchMode == SearchMode.Delayed)
            {
                //TODO RaiseSearchEvent();
            }

            IsMouseLeftButtonDown = false;
        }





        public static DependencyProperty SearchEventTimeDelayProperty =
            DependencyProperty.Register(
            "SearchEventTimeDelay",
            typeof(Duration),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(
                new Duration(new TimeSpan(0, 0, 0, 0, 500)),
                new PropertyChangedCallback(OnSearchEventTimeDelayChanged)));

        public Duration SearchEventTimeDelay
        {
            get { return (Duration)GetValue(SearchEventTimeDelayProperty); }
            set { SetValue(SearchEventTimeDelayProperty, value); }
        }

        static void OnSearchEventTimeDelayChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SearchTextBox stb = o as SearchTextBox;
            if (stb != null)
            {
                stb.searchEventDelayTimer.Interval = ((Duration)e.NewValue).TimeSpan;
                stb.searchEventDelayTimer.Stop();
            }
        }


        public static readonly RoutedEvent SearchEvent =
            EventManager.RegisterRoutedEvent(
                "Search",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SearchTextBox));


        public event RoutedEventHandler Search
        {
            add { AddHandler(SearchEvent, value); }
            remove { RemoveHandler(SearchEvent, value); }
        }


        private void RaiseSearchEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(SearchEvent);
            RaiseEvent(args);
        }


        private DispatcherTimer searchEventDelayTimer;

        public SearchTextBox() : base()
        {
            searchEventDelayTimer = new DispatcherTimer();
            searchEventDelayTimer.Interval = SearchEventTimeDelay.TimeSpan;
            searchEventDelayTimer.Tick += new EventHandler(OnSeachEventDelayTimerTick);
        }


        void OnSeachEventDelayTimerTick(object o, EventArgs e)
        {
            searchEventDelayTimer.Stop();
            RaiseSearchEvent();
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#pragma warning disable RCS1226 // Add paragraph to documentation comment.

namespace PsControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PsControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PsControls;assembly=PsControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:RatingControl/>
    ///
    /// </summary>
    public class RatingControl : Control
    {
        static RatingControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingControl), new FrameworkPropertyMetadata(typeof(RatingControl)));
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption",
                                                    typeof(string), typeof(RatingControl),
                                                    new FrameworkPropertyMetadata()
                                                    {
                                                        DefaultValue = string.Empty,
                                                        BindsTwoWayByDefault = false
                                                    });
        public static readonly DependencyProperty InitialSetValueProperty = DependencyProperty.Register("InitialSetValue",
                                                    typeof(int), typeof(RatingControl),
                                                    new FrameworkPropertyMetadata()
                                                    {
                                                        DefaultValue = 1,
                                                        BindsTwoWayByDefault = false
                                                    });
        public static readonly DependencyProperty IsClearEnabledProperty = DependencyProperty.Register("IsClearEnabled",
                                                    typeof(bool), typeof(RatingControl),
                                                    new FrameworkPropertyMetadata()
                                                    {
                                                        DefaultValue = true,
                                                        BindsTwoWayByDefault = false
                                                    });
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly",
                                                    typeof(bool), typeof(RatingControl),
                                                    new FrameworkPropertyMetadata()
                                                    {
                                                        DefaultValue = false,
                                                        BindsTwoWayByDefault = false
                                                    });
        public static readonly DependencyProperty MaxRatingProperty = DependencyProperty.Register("MaxRating",
                                                    typeof(int), typeof(RatingControl),
                                                    new FrameworkPropertyMetadata()
                                                    {
                                                        DefaultValue = 5,
                                                        BindsTwoWayByDefault = false
                                                    });
        public static readonly DependencyProperty PlaceholderValueProperty = DependencyProperty.Register("PlaceholderValue",
                                                    typeof(double?), typeof(RatingControl),
                                                    new FrameworkPropertyMetadata()
                                                    {
                                                        DefaultValue = null,
                                                        BindsTwoWayByDefault = false
                                                    });
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
                                                    typeof(double), typeof(RatingControl),
                                                    new FrameworkPropertyMetadata()
                                                    {
                                                        DefaultValue = 0d,
                                                        BindsTwoWayByDefault = true,
                                                        DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                                                    });

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public int InitialSetValue
        {
            get { return (int)GetValue(InitialSetValueProperty); }
            set { SetValue(InitialSetValueProperty, value); }
        }

        public bool IsClearEnabled
        {
            get { return (bool)GetValue(IsClearEnabledProperty); }
            set { SetValue(IsClearEnabledProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public int MaxRating
        {
            get { return (int)GetValue(MaxRatingProperty); }
            set { SetValue(MaxRatingProperty, value); }
        }

        public double? PlaceholderValue
        {
            get { return (double?)GetValue(PlaceholderValueProperty); }
            set { SetValue(PlaceholderValueProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}

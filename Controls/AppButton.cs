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

namespace PsControls.Controls
{
    /// <inheritdoc />
    public class AppButton : MenuItem
    {
        static AppButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppButton), new FrameworkPropertyMetadata(typeof(AppButton)));
        }
    }
}

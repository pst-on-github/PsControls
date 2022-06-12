using System;
using System.Windows;
using System.Windows.Controls;

#pragma warning disable RCS1226 // Add paragraph to documentation comment.

namespace PsControls.Controls
{
    /// <inheritdoc />
    public class CommandBar : Menu
    {
        static CommandBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandBar), new FrameworkPropertyMetadata(typeof(CommandBar)));
        }
    }
}

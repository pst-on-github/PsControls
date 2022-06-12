using System;
using System.Windows;
using System.Windows.Controls;

#pragma warning disable RCS1226 // Add paragraph to documentation comment.

namespace PsControls.Controls
{
    /// <inheritdoc />
    public class CommandBarItem : MenuItem
    {
        /// <summary>
        /// DependencyProperty for <see cref="IsMetroIcon" /> property.
        /// </summary>
        public static readonly DependencyProperty IsMetroIconProperty =
            DependencyProperty.Register(
                "IsMetroIcon",
                typeof(bool),
                typeof(CommandBarItem),
                new PropertyMetadata(true));

        static CommandBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandBarItem), new FrameworkPropertyMetadata(typeof(CommandBarItem)));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the header text will display METRO icons.
        /// </summary>
        public bool IsMetroIcon
        {
            get { return (bool)GetValue(IsMetroIconProperty); }
            set { SetValue(IsMetroIconProperty, value); }
        }
    }
}

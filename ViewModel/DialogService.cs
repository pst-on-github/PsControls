using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PsControls.ViewModel
{
    /// <summary>
    /// Provides a logic to handle dialogs.
    /// </summary>
    public static class DialogService
    {
        private static readonly Dictionary<BaseDialogViewModel, Window> _knownVms = new ();

        /// <summary>
        /// Attaches a view models as content to the given dialog.
        /// </summary>
        /// <param name="content">The content to be shown in the dialog.</param>
        public static void Attach(BaseDialogViewModel content)
        {
            if (!_knownVms.ContainsKey(content))
            {
                // Don't do anything yet, but listen to IsOpen changes
                _knownVms.Add(content, null);
                content.PropertyChanged += DialogVM_PropertyChanged;
                if (content.IsOpen)
                {
                    // It is already supposed to be open. Open it now.
                    _knownVms[content] = CreateDialogWindow(content);
                    _knownVms[content].Show();
                }
            }
        }

        private static void DialogVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOpen" && sender is BaseDialogViewModel dialogVm)
            {
                if (dialogVm.IsOpen)
                {
                    _knownVms[dialogVm] = CreateDialogWindow(dialogVm);
                    _knownVms[dialogVm].Show();
                }
                else
                {
                    var win = _knownVms[dialogVm];
                    win.Loaded -= Window_Loaded;
                    win.ContentRendered -= Window_ContentRendered;
                    win.Closed -= Window_Closed;
                    _knownVms[dialogVm].Close();
                    _knownVms.Remove(dialogVm);
                }
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            if (sender is Window win)
            {
                if (win.Content is BaseDialogViewModel dialogVm)
                {
                    dialogVm.IsOpen = false;
                }

                // Setting the owner and giving it the focus prevents
                // main window put in background after closing a child window
                // See: https://stackoverflow.com/questions/21790062/wpf-main-window-put-in-background-after-closing-a-child-window
                win.Owner?.Focus();
            }
        }

        private static void Window_Loaded(object sender, EventArgs e)
        {
            if (sender is Window win)
            {
                win.MoveFocus(new TraversalRequest(FocusNavigationDirection.First)); // First focus
            }
        }

        private static void Window_ContentRendered(object sender, EventArgs e)
        {
            if (sender is Window win)
            {
                RefitAndLockWindowSize(win);

                // Run a second pass in case template/data changes are still queued.
                win.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        RefitAndLockWindowSize(win);
                    }),
                    System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                win.ContentRendered -= Window_ContentRendered;
            }
        }

        private static void RefitAndLockWindowSize(Window win)
        {
            win.MinHeight = 0;
            win.MinWidth = 0;

            win.SizeToContent = SizeToContent.WidthAndHeight;
            win.UpdateLayout();

            double fittedHeight = win.ActualHeight;
            double fittedWidth = win.ActualWidth;

            win.SizeToContent = SizeToContent.Manual;
            win.Height = fittedHeight;
            win.Width = fittedWidth;
            win.MinHeight = fittedHeight;
            win.MinWidth = fittedWidth;
        }

        private static Window CreateDialogWindow(BaseDialogViewModel content)
        {
            var win = new Window
            {
                Content = content,
                Owner = Application.Current.MainWindow,
                ResizeMode = ResizeMode.CanResizeWithGrip, // ResizeMode.NoResize,
                ShowActivated = true,
                ShowInTaskbar = false,
                SizeToContent = SizeToContent.WidthAndHeight,
                Title = Application.Current.Properties["ProductName"].ToString(),
                WindowStyle = WindowStyle.ToolWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            win.Closed += Window_Closed;
            win.Loaded += Window_Loaded;
            win.ContentRendered += Window_ContentRendered;

            return win;
        }
    }
}

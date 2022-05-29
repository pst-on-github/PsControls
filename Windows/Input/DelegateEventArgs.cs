using System;
using System.Windows.Input;

namespace PsControls.Windows.Input
{
    public class DelegateEventArgs : EventArgs
    {
        public DelegateEventArgs(object commandParameter)
        {
            Parameter = commandParameter;
        }

        public object Parameter { get; }
    }
}

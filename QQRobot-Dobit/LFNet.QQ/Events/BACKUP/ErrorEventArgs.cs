
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Events
{
    public class ErrorEventArgs : EventArgs
    {
        public string PortName { get; set; }
        public Exception Exception { get; private set; }
        public ErrorEventArgs(Exception e)
        {
            this.Exception = e;
        }
        public ErrorEventArgs(Exception e, string portName)
        {
            this.Exception = e;
            this.PortName = portName;
        }
    }
}

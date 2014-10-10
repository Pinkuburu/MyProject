using System;
using Microsoft.Win32;
using System.ServiceProcess;

namespace SystemMonitor
{
    class Program
    {
        static void Main(string[] args)
        {

            //SystemEvents.EventsThreadShutdown += new EventHandler(SystemEvents_EventsThreadShutdown);
            SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);

            Console.WriteLine("This application is waiting for system events.");
            Console.WriteLine("Press <Enter> to terminate this application.");
            Console.ReadLine();
        }

        static void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            SessionChangeDescription sss = new SessionChangeDescription();

            Console.WriteLine("Time Change");
        }
    }
}

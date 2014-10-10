using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace VPN {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            Application.Run(new Form1());
        }

        static void Application_ApplicationExit(object sender , EventArgs e) {
            SerializHelper.Save();
        }

        static void CurrentDomain_UnhandledException(object sender , UnhandledExceptionEventArgs e) {
            try {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                    "with the following information:\n\n";

                // Since we can't prevent the app from terminating, log this to the event log.
                if( !EventLog.SourceExists("ThreadException") ) {
                    EventLog.CreateEventSource("ThreadException" , "Application");
                }

                // Create an EventLog instance and assign its source.
                EventLog myLog = new EventLog();
                myLog.Source = "ThreadException";
                myLog.WriteEntry(errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
            } catch( Exception exc ) {
                try {
                    MessageBox.Show("Fatal Non-UI Error" ,
                        "Fatal Non-UI Error. Could not write the error to the event log. Reason: "
                        + exc.Message , MessageBoxButtons.OK , MessageBoxIcon.Stop);
                } finally {
                    Application.Exit();
                }
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            MessageBox.Show(e.Exception.Message);
        }
    }
}

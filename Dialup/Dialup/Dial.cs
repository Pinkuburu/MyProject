using System.Diagnostics;
using System.IO;
using System;

namespace Dialup
{
    class Dial
    {
        public void Connect(string connectionName, string user, string pass)
        {
            string arg = string.Format("rasdial {0} {1} {2}", connectionName, user, pass);
            InvokeCmd(arg);
        }

        public void Disconnect(string connectionName)
        {
            string arg = string.Format("rasdial {0} /disconnect", connectionName);
            InvokeCmd(arg);
        }
        private static string InvokeCmd(string cmdArgs)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            p.StandardInput.WriteLine(cmdArgs);
            p.StandardInput.WriteLine("exit");
            //StreamReader output = p.StandardOutput;
            //Console.WriteLine(output.ReadToEnd());

            return p.StandardOutput.ReadToEnd();
        }
    }
}

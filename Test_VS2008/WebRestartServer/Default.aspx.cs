using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WebRestartServer
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Write(CMD("c:\\shutdown -r -f"));
        }

        #region 命令控制台调用 CMD(string dosCommand)
        /// <summary>
        /// 命令控制台调用
        /// </summary>
        /// <param name="dosCommand"></param>
        /// <returns></returns>
        public int CMD(string dosCommand)
        {
            int errorLevel = -1;
            //string pathToScannerProgram = Path.Combine(virusCheckFolder, "scan.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + dosCommand;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                //process.StandardOutput.ReadToEnd();
                string output = process.StandardOutput.ReadToEnd();
                Response.Write(output);
                errorLevel = process.ExitCode;//返回ERRORLEVEL
                process.WaitForExit();
            }
            return errorLevel;
        }
        #endregion 命令控制台调用 CMD(string dosCommand)
    }
}

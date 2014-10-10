using System;
using System.Windows.Forms;
using System.IO;

namespace OperationHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strSQL = "-- 2010-3-12/16:21 上生成的脚本\r\n"
                            + "-- 由: Powered by Cupid\r\n"
                            + "-- 服务器: (LOCAL)\r\n\r\n"
                            + "BEGIN TRANSACTION\r\n"
                            + "  DECLARE @JobID BINARY(16)\r\n"
                            + "  DECLARE @ReturnCode INT\r\n"
                            + "  SELECT @ReturnCode = 0\r\n"
                            + "IF (SELECT COUNT(*) FROM msdb.dbo.syscategories WHERE name = N'[Uncategorized (Local)]') < 1\r\n"
                            + "  EXECUTE msdb.dbo.sp_add_category @name = N'[Uncategorized (Local)]'\r\n\r\n"
                            + "  -- 删除同名的警报（如果有的话）。\r\n"
                            + "  SELECT @JobID = job_id\r\n"
                            + "  FROM   msdb.dbo.sysjobs\r\n"
                            + "  -- ========== * 作业名称 * ==========\r\n"
                            + "  WHERE (name = N'" + textOName.Text.Trim() + "')\r\n"
                            + "  -- ========== * 作业名称 * ==========\r\n"
                            + "  IF (@JobID IS NOT NULL)\r\n"
                            + "  BEGIN\r\n"
                            + "  -- 检查此作业是否为多重服务器作业\r\n"
                            + "  IF (EXISTS (SELECT  *\r\n"
                            + "              FROM    msdb.dbo.sysjobservers\r\n"
                            + "              WHERE   (job_id = @JobID) AND (server_id <> 0)))\r\n"
                            + "  BEGIN\r\n"
                            + "    -- 已经存在，因而终止脚本\r\n"
                            + "    RAISERROR (N'无法导入作业“" + textOName.Text.Trim() + "”，因为已经有相同名称的多重服务器作业。', 16, 1)\r\n"
                            + "    GOTO QuitWithRollback\r\n"
                            + "  END\r\n"
                            + "  ELSE\r\n"
                            + "    -- 删除［本地］作业\r\n"
                            + "    EXECUTE msdb.dbo.sp_delete_job @job_name = N'" + textOName.Text.Trim() + "'\r\n"
                            + "    SELECT @JobID = NULL\r\n"
                            + "  END\r\n\r\n"
                            + "BEGIN\r\n"
                            + "  -- 添加作业\r\n"
                            + "  EXECUTE @ReturnCode = msdb.dbo.sp_add_job @job_id = @JobID OUTPUT , @job_name = N'" + textOName.Text.Trim() + "', @owner_login_name = N'sa', @description = N'没有可用的描述。', @category_name = N'[Uncategorized (Local)]', @enabled = 1, @notify_level_email = 0, @notify_level_page = 0, @notify_level_netsend = 0, @notify_level_eventlog = 2, @delete_level= 0\r\n"
                            + "  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback\r\n\r\n"
                            + "  -- 添加作业步骤\r\n"
                            + "  EXECUTE @ReturnCode = msdb.dbo.sp_add_jobstep @job_id = @JobID, @step_id = 1, @step_name = N'step1', @command = N'" + textScript.Text.Trim() + ""
                            + "', @database_name = N'NewBTP', @server = N'', @database_user_name = N'', @subsystem = N'TSQL', @cmdexec_success_code = 0, @flags = 0, @retry_attempts = 0, @retry_interval = 1, @output_file_name = N'', @on_success_step_id = 0, @on_success_action = 1, @on_fail_step_id = 0, @on_fail_action = 2\r\n"
                            + "  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback\r\n"
                            + "  EXECUTE @ReturnCode = msdb.dbo.sp_update_job @job_id = @JobID, @start_step_id = 1\r\n\r\n"
                            + "  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback\r\n\r\n"
                            + "  -- 添加作业调度\r\n"
                            + "  EXECUTE @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id = @JobID, @name = N'runtime', @enabled = 1, @freq_type = 1, @active_start_date = " + dateTimePicker1.Value.ToString("yyyyMMdd") + ", @active_start_time = " + dateTimePicker1.Value.ToString("HHmmss") + "\r\n"
                            + "  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback\r\n\r\n"
                            + "  -- 添加目标服务器\r\n"
                            + "  EXECUTE @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @JobID, @server_name = N'(local)'\r\n"
                            + "  IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback\r\n\r\n"
                            + "END\r\n"
                            + "COMMIT TRANSACTION\r\n"
                            + "GOTO   EndSave\r\n"
                            + "QuitWithRollback:\r\n"
                            + "  IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION\r\n"
                            + "EndSave: \r\n";

            //textScript.Text = dateTimePicker1.Value.ToString("yyyyMMdd");
            //textScript.Text += dateTimePicker1.Value.ToString("HHmmss");
            //textScript.Text = strSQL;
            if (textOName.Text.Trim() == "")
            {
                MessageBox.Show("作业名称不能为空！");
            }
            else
            {
                CreateTxt(strSQL,textFileName.Text.Trim());
            }
        }

        #region 生成特定文件格式的TXT文档
        /// <summary>
        /// 生成特定文件格式的
        /// </summary>
        /// <param name="strContent"></param>
        private static void CreateTxt(string strContent,string strFileName)//生成特定文件格式的TXT文档
        {
            Random rnbNum = new Random();
            DateTime dt = DateTime.Now;
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;
            MessageBox.Show(strPath + " " + dt.ToString("yyyy-MM-dd"));
            string strNum = Convert.ToString(rnbNum.Next(000000, 999999));
            if (strFileName == "")
            {
                strFileName = dt.ToString("yyyy-MM-dd")+" "+strNum;
            }            
            FileStream fs = new FileStream(@strPath + strFileName + ".sql", FileMode.OpenOrCreate, FileAccess.Write);
            //FileStream fs = new FileStream(@"C:\fetion2009\commands\" + filename + ".cmd", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//通过指定字符编码方式可以实现对汉字的支持，否则在用记事本打开查看会出现乱码

            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.WriteLine(strContent);
            sw.Flush();
            sw.Close();
        }
        #endregion
    }
}

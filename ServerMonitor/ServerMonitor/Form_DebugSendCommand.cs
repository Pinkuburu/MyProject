using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ServerMonitor
{
    public partial class Form_DebugSendCommand : Form
    {
        public string strCommand;

        public Form_DebugSendCommand()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.strCommand = textBox_Command.Text.Trim();
        }
    }
}

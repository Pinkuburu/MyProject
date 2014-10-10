using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace CPU测试
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PerformanceCounterFun(string CategoryName, string InstanceName, string CounterName)
        {
            PerformanceCounter pc = new PerformanceCounter(CategoryName, CounterName, InstanceName);
            while (true)
            {
                Thread.Sleep(1000);
                float cpuLoad = pc.NextValue();
                progressBar1.Value = Convert.ToInt32(cpuLoad);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PerformanceCounterFun("Processor", "_Total", "% Processor Time");
        }
    }
}
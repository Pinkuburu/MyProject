using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text;

namespace Serial_Control_LED
{
    public partial class Form1 : Form
    {
        public SerialPort COM_CONN;
        public delegate void SetTextBox_Debug(string str);  //委托

        public Form1()
        {
            InitializeComponent();
            label_Status.Text = "尚未连接";
        }

        private void button_ON_Click(object sender, EventArgs e)
        {
            this.COM_CONN.Write("1");
        }

        private void button_OFF_Click(object sender, EventArgs e)
        {
            this.COM_CONN.Write("0");
        }

        private void button_BLINK_Click(object sender, EventArgs e)
        {
            this.COM_CONN.Write("2");
        }

        private void comboBox_COM_DropDown(object sender, EventArgs e)
        {
            foreach (string COM_NAME in SerialPort.GetPortNames())
            {
                if (!Check_COM_NAME(COM_NAME))
                {
                    comboBox_COM.Items.Add(COM_NAME);
                }                
            }
        }

        private bool Check_COM_NAME(string COM_NAME)
        {
            foreach (string TEMP_NAME in comboBox_COM.Items)
            {
                if (COM_NAME == TEMP_NAME)
                {
                    return true;
                }
            }
            return false;
        }

        private void button_Conn_Click(object sender, EventArgs e)
        {
            string COM_NAME = comboBox_COM.SelectedItem.ToString();
            if(COM_NAME != "")
            {
                this.COM_CONN = new SerialPort(COM_NAME, 9600);
                this.COM_CONN.Open();
                this.COM_CONN.DataReceived += new SerialDataReceivedEventHandler(COM_CONN_DataReceived);

                if (this.COM_CONN.IsOpen == true)
                {
                    label_Status.Text = COM_NAME + "串口已经连接";
                }
                else
                {
                    label_Status.Text = COM_NAME + "串口已关闭";
                }    
            }            
        }

        void COM_CONN_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DebugMSG(this.COM_CONN.ReadLine());
        }

        /// <summary>
        /// Web调试信息
        /// </summary>
        /// <param name="strContent">显示内容</param>
        private void DebugMSG(string strContent)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextBox_Debug setBox_Debug = new SetTextBox_Debug(DebugMSG);
                this.textBox1.Invoke(setBox_Debug, strContent);
            }
            else
            {
                textBox1.Text += strContent + "\r\n";
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SVS
{
    public partial class Demo : Form
    {
        public Demo()
        {
            InitializeComponent();
        }

        private void InitializeWindowPosition(ref int pos_x)
        {
            // set to the rightest screen
            Screen[] all_screen = Screen.AllScreens;
            if (all_screen.Length > 1)
            {
                foreach (Screen scr in all_screen)
                {
                    if (pos_x < scr.WorkingArea.Left)
                    {
                        pos_x = scr.WorkingArea.Left;
                    }
                }
            }
        }

        public delegate void ReturnValueDelegate();

        public event ReturnValueDelegate ReturnPointFormNull;

        private void realTimeStylusPluginsControl1_ReturnXYZCallback(string str)
        {
            myTextUI(str);
        }

        private delegate void TextCallBack(string str);

        private void myTextUI(string str)
        {
            if (this.InvokeRequired)
            {
                TextCallBack myUpdate = new TextCallBack(myTextUI);
                this.Invoke(myUpdate, str);
            }
            else
            {
                label2.Text = str;
            }
        }

        private void realTimeStylusPluginsControl1_ReturnDataCallback(int x,int y,int z)
        {
            myProgressbarUI(z);            
        }

        private delegate void ValueCallBack(int value);

        private void myProgressbarUI(int value)
        {
            if (this.InvokeRequired)
            {
                ValueCallBack myUpdate = new ValueCallBack(myProgressbarUI);
                this.Invoke(myUpdate, value);
            }
            else
            {
                if (value < progressBar1.Minimum) value = progressBar1.Minimum;
                if (value > progressBar1.Maximum) value = progressBar1.Minimum;
                progressBar1.Value = value;
            }
        }

        private void Demo_Load(object sender, EventArgs e)
        {
            int pos_x = 0;
            InitializeWindowPosition(ref pos_x);
            this.Left = pos_x;
            this.WindowState = FormWindowState.Maximized;

            uint deviceID = 1;

            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE dm = new DEVMODE();
            d.cb = Marshal.SizeOf(d);

            NativeMethods.EnumDisplayDevices(null, deviceID, ref d, 0);
            if (0 != NativeMethods.EnumDisplaySettings(
                d.DeviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm))
            {
                label3.Text = "Resolution : " + dm.dmPelsWidth.ToString() + "x" + dm.dmPelsHeight.ToString();
            }
            else
            {
                deviceID = 0;
                NativeMethods.EnumDisplayDevices(null, deviceID, ref d, 0);
                if (0 != NativeMethods.EnumDisplaySettings(
                    d.DeviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm))
                {
                    label3.Text = "Resolution : " + dm.dmPelsWidth.ToString() + "x" + dm.dmPelsHeight.ToString();
                }
            }
            realTimeStylusPluginsControl1.Init();
            realTimeStylusPluginsControl1.ReturnXYZCallback += realTimeStylusPluginsControl1_ReturnXYZCallback;
            realTimeStylusPluginsControl1.ReturnDataCallback +=realTimeStylusPluginsControl1_ReturnDataCallback;
            progressBar1.Maximum = 2048;
        }        

        public bool SaveImage()
        {
            realTimeStylusPluginsControl1.BringToFront();            
            realTimeStylusPluginsControl1.SaveImage();
            label1.BringToFront();
            label2.BringToFront();
            label3.BringToFront();
            progressBar1.BringToFront();
            return true;
        }

        public bool ChangePenColor(Color c)
        {
            realTimeStylusPluginsControl1.PenColor = c;
            return true;
        }

        public void ClearImage()
        {
            realTimeStylusPluginsControl1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            ReturnPointFormNull();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearImage();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            if(SaveImage())
            {
                MessageBox.Show("Save OK", "Information");
                this.Close();
                this.Dispose();
            }
        }

        private void Demo_FormClosing(object sender, FormClosingEventArgs e)
        {
    
        }
    }
}

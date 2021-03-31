using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.Quartz.Net.Jobs;

namespace WindowsFormsApp.Quartz.Net
{
    public partial class Form1 : Form
    {
        Scheduler scheduler = new Scheduler();

        public Form1()
        {
            InitializeComponent();
            scheduler.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.cnblogs.com/javahr/p/8318728.html");
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("你确定要关闭吗！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                e.Cancel = false;  //点击OK   
                //在应用程序关闭时运行的代码
                if (scheduler._scheduler != null)
                {
                    scheduler._scheduler.Shutdown(true);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}

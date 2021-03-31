using Net.Framework.Log;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            NLogHelper.Info(Guid.NewGuid().ToString(), "----开始----","","");
            //var result =  Save();
            //var cc = await result;
            //MessageBox.Show(cc);
            NLogHelper.Info(Guid.NewGuid().ToString(), "----结束----");

        }




        public async Task<string> Save()
        {

            var webclient = new WebClient();
            var result = await webclient.DownloadStringTaskAsync("https://stackoverflow.com/");

            string path = string.Empty;
            //新建一个线程
            await Task.Run(() =>
            {
                File.WriteAllText(Directory.GetCurrentDirectory(), result);
                path = Path.GetFullPath(Directory.GetCurrentDirectory());
            });

            return path;

        }
    }
}

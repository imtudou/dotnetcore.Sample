using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf4
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //调用系统图片查看器
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //查看原图
            try
            {
                System.Drawing.Image image = null;
                //System.Drawing.Image image = System.Drawing.Image.FromFile(oldFilename);
                var tempFilePath = string.Empty;
                if (image != null)
                {
                    //tempFilePath = System.Windows.Forms.Application.StartupPath + "\\temp.png";
                   

                    Bitmap bm = new Bitmap(image);
                    bm.Save(tempFilePath);
                    bm.Dispose();
                }
                tempFilePath = @"D:\HD\lizy\Desktop\test\PTBX1910030003_通用类报销单_0_\PTBX1910030003_通用类报销单_facesheet_1.jpg";
                //建立新的系统进程      
                System.Diagnostics.Process process = new System.Diagnostics.Process();

                //设置图片的真实路径和文件名      
                process.StartInfo.FileName = tempFilePath;

                //设置进程运行参数，这里以最大化窗口方法显示图片。      
                process.StartInfo.Arguments = "rundl132.exe C://WINDOWS//system32//shimgvw.dll,ImageView_Fullscreen";

                //此项为是否使用Shell执行程序，因系统默认为true，此项也可不设，但若设置必须为true      
                process.StartInfo.UseShellExecute = true;

                //此处可以更改进程所打开窗体的显示样式，可以不设      
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.Close();
            }
            catch
            {

            }


        }
    }
}

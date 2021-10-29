using ScanTitle.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanTitle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //条码扫描时，回车键触发
       
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                txtQRcode.Focus();
            }
        }
       

        private void txtQRcode_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                Scan();
            }
           
        }
        private void Scan()
        {
            //为了模拟扫码枪扫描就直接将扫描出来的信息赋值了
            this.txtQRcode.Text = "https://mp.weixin.qq.com/intp/invoice/usertitlewxa?action=list_auth&invoice_key=Y201MnJOWXBNbVUzRURhaDlSN1UhaThuaDpvR1dTMyZKVCJjektPfSVmUjJeXlFKbDRPO2d2OzJHbDk#wechat_redirect";
            if (txtQRcode.Text.StartsWith("https"))
            {
                var scanText = txtQRcode.Text;
                string scantitle = WeChatInvoice.GetScanTitle(scanText);
                this.listBox1.Text = scanText;
            }
        }
    }
}

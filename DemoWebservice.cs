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

namespace NT106_Demo
{
    public partial class DemoWebservice : Form
    {
        public DemoWebservice()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                string publicIP = client.DownloadString("http://api.ipify.org");
                label1.Text = "Your public IP is: " + publicIP;
            }
        }
    }
}

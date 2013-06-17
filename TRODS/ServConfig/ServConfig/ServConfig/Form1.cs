using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using EugLib;

namespace ServConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            reload();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                /*IPHostEntry ipHostEntry = Dns.GetHostByName(Dns.GetHostName());
                IPAddress ipAddress = ipHostEntry.AddressList[0];*/
                textBox1.Text = new WebClient().DownloadString("http://demango.ovh.org/adresseip.php");
            }
            catch (Exception)
            {
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 25564;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EugLib.IO.FileStream.writeFile("files/server", textBox1.Text + ':' + numericUpDown1.Value.ToString());
            Environment.Exit(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            reload();
        }

        public void reload()
        {
            try
            {
                string[] tab = EugLib.IO.FileStream.readFile("files/server").Split(':');
                if (tab.Length >= 2)
                {
                    textBox1.Text = tab[0];
                    numericUpDown1.Value = int.Parse(tab[1]);
                }
            }
            catch (Exception er)
            {
                System.Windows.Forms.MessageBox.Show(er.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "127.0.0.1";
        }
    }
}

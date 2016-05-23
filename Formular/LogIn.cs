using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formular
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BDUtilizatoriDataSetTableAdapters.UtilizatoriTableAdapter utilizatori = new BDUtilizatoriDataSetTableAdapters.UtilizatoriTableAdapter();
            BDUtilizatoriDataSet.UtilizatoriDataTable DatatableUtilizatori = utilizatori.GetDataByUsername();

           

            foreach (DataRow row in DatatableUtilizatori.Rows)
            {
                if(textBox1.Text.Equals(row[1].ToString()))
                {
                    Byte[] base64EncodedBytes = System.Convert.FromBase64String(row[2].ToString());
                    string parola = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                    if (textBox2.Text.Equals(parola))
                    {

                        this.Hide();
                        Form1 MainWindowApp = new Form1();
                        MainWindowApp.Show();
                        
                    }
                }
            }
          

           
        }
    }
}

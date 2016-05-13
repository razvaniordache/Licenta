using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formular
{
    public partial class Form1 : Form
    {
        static List<Models.Product> availableProducts = new List<Models.Product>();
        Fridge frigider = new Fridge();
        static List<Models.Product> wishList = new List<Models.Product>();

        public Form1()
        {
            InitializeComponent();
            Fridge frigider = new Fridge();


            ListViewItem lvi = new ListViewItem("Lapte Zuzu");
            Models.Product lapteZuzu = new Models.Product("Lapte Zuzu", 1000, "ml");
            availableProducts.Add(lapteZuzu);
            lvi.SubItems.Add("1000");
            lvi.SubItems.Add("ml");

            ListViewItem lvi2 = new ListViewItem("Branza Kiri");
            Models.Product kiri = new Models.Product("Branza Kiri", 100, "grams");
            availableProducts.Add(kiri);
            lvi2.SubItems.Add("100");
            lvi2.SubItems.Add("grams");
            listView1.Items.Add(lvi);
            listView1.Items.Add(lvi2);

            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (Models.Product prod in availableProducts)
            {

                this.chart1.Series["Quantity"].Points.AddXY(prod.Name, prod.Quantity);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            string resp = frigider.checkProdus(textBoxCod.Text).ToString();
           
      

            ListViewItem item;
            bool exists = false;
            //MessageBox.Show(resp);
            richTextBox1.Text = "";
            if (resp.Equals("Invalid"))
            {

                richTextBox1.Text = "Produsul nu este recunoscut";
            }
            else
            {
                foreach (Models.Product prod in availableProducts)
                {
                    //Already in the list just increse the quantity 
                    if (prod.Name.Equals(resp) && textBox1.Text.Length != 0 && float.Parse(textBox1.Text) > 1)
                    {
                        prod.Quantity = prod.Quantity + int.Parse(textBox1.Text);
                        listView1.Items.Clear();
                        foreach (Models.Product prod2 in availableProducts)
                        {
                            item = new ListViewItem(new string[] { prod2.Name, prod2.Quantity.ToString(), prod2.UnitOfMeasure });
                            listView1.Items.Add(item);

                        }
                        exists = true;
                        try
                        {
                            string aboutProdus = frigider.GetRequest(textBoxCod.Text);
                            richTextBox1.Text = aboutProdus;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                        
                    }
                }
                //New product in the list
                if (!exists && textBox1.Text.Length != 0 && float.Parse(textBox1.Text)>1 && comboBox1.SelectedIndex>-1)
                {
                    Models.Product produsNou = new Models.Product(resp, int.Parse(textBox1.Text), comboBox1.Text);
                    availableProducts.Add(produsNou);
                    item = new ListViewItem(new string[] { resp, textBox1.Text, comboBox1.Text });
                    listView1.Items.Add(item);
                    try
                    {
                        string aboutProdus = frigider.GetRequest(textBoxCod.Text);
                        richTextBox1.Text = aboutProdus;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else if (textBox1.Text.Length == 0 || float.Parse(textBox1.Text) < 1 || comboBox1.Text.Length==0)
                {
                    richTextBox1.Text = "Nu ati indeplinit una din conditii"+Environment.NewLine+"-Nu ati completat unul din campuri"
                                            + Environment.NewLine + "-Cantitatea trebuie sa fie supraunitara";
                }
            }

            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (Models.Product prod in availableProducts)
            {
                
                this.chart1.Series["Quantity"].Points.AddXY(prod.Name,prod.Quantity);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string resp = frigider.checkProdus(textBoxCod.Text).ToString();
            ListViewItem item;
            richTextBox1.Text = "";
            if (resp.Equals("Invalid"))
            {
                richTextBox1.Text = "Produsul nu este recunoscut";
            }
            else
            {
                textBox4.Text = resp;
                foreach (Models.Product prod in availableProducts)
                {
                    if (prod.Name.Equals(resp) && float.Parse(textBox3.Text)>1 )
                    {
                        textBox4.Text = resp;
                        if (prod.DefaultQuantity < prod.Quantity - int.Parse(textBox3.Text))
                        {
                            prod.Quantity = prod.Quantity - int.Parse(textBox3.Text);
                            listView1.Items.Clear();
                            foreach (Models.Product prod2 in availableProducts)
                            {
                                item = new ListViewItem(new string[] { prod2.Name, prod2.Quantity.ToString(), prod2.UnitOfMeasure });
                                listView1.Items.Add(item);
                            }
                        }
                        else
                        {
                            richTextBox1.Text = "Cantitate insuficienta de "+resp;
                            int WishListQuantity = prod.Quantity - int.Parse(textBox3.Text);
                            WishListQuantity = WishListQuantity * (-1);
                            prod.Quantity = prod.DefaultQuantity;
                            listView1.Items.Clear();
                            foreach (Models.Product prod2 in availableProducts)
                            {
                                item = new ListViewItem(new string[] { prod2.Name, prod2.Quantity.ToString(), prod2.UnitOfMeasure });
                                listView1.Items.Add(item);
                            }

                            DialogResult dialogResult = MessageBox.Show("Would you like to add"+prod.Name+" to Wish List ?","No more " +prod.Name, MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                wishList.Add(prod);
                                item = new ListViewItem(new string[] { prod.Name, (WishListQuantity).ToString() });
                                listView2.Items.Add(item);
                            }
                            else
                            {
                                richTextBox1.Text = " Se lanseaza cautarea online ... ";
                                frigider.LaunchOnlineCommand(resp);
                            }
                            
                        }
                    }
                } 
            }

            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (Models.Product prod in availableProducts)
            {

                this.chart1.Series["Quantity"].Points.AddXY(prod.Name, prod.Quantity);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
           
        }
    }
}

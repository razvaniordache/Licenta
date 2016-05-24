using Formular.Notifications;
using Formular.Notifications.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

            //Produse Default din frigider
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

            ListViewItem lvi3 = new ListViewItem("Unt Jager");
            Models.Product Unt = new Models.Product("Unt Jager", 400, "grams");
            availableProducts.Add(Unt);
            lvi3.SubItems.Add("400");
            lvi3.SubItems.Add("grams");
            listView1.Items.Add(lvi3);

            ListViewItem lvi4 = new ListViewItem("Delaco Branza Verdeata");
            Models.Product Delaco = new Models.Product("Delaco Branza Verdeata", 300, "grams");
            availableProducts.Add(Delaco);
            lvi4.SubItems.Add("300");
            lvi4.SubItems.Add("grams");
            listView1.Items.Add(lvi4);

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
                if (!exists && textBox1.Text.Length != 0 && float.Parse(textBox1.Text) > 1 && comboBox1.SelectedIndex > -1)
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
                else if (textBox1.Text.Length == 0 || float.Parse(textBox1.Text) < 1 || comboBox1.Text.Length == 0)
                {
                    richTextBox1.Text = "Nu ati indeplinit una din conditii" + Environment.NewLine + "-Nu ati completat unul din campuri"
                                            + Environment.NewLine + "-Cantitatea trebuie sa fie supraunitara";
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
                    if (prod.Name.Equals(resp) && textBox3.Text.Length>0 && float.Parse(textBox3.Text) > 1)
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
                            richTextBox1.Text = "Cantitate insuficienta de " + resp;
                            DateTime localDate = DateTime.Now;
                            new PushBullet().SendNotification(new Notifications.Models.Notification()
                            {
                                Title = "Run out of " + resp,
                                Body = "Run out of "+resp+" at "+localDate.ToString(),
                                Type = PushBulletTypes.Note.ToString().ToLower()
                            });

                            int WishListQuantity = prod.Quantity - int.Parse(textBox3.Text);
                            WishListQuantity = WishListQuantity * (-1);
                            prod.Quantity = prod.DefaultQuantity;
                            listView1.Items.Clear();
                            foreach (Models.Product prod2 in availableProducts)
                            {
                                item = new ListViewItem(new string[] { prod2.Name, prod2.Quantity.ToString(), prod2.UnitOfMeasure });
                                listView1.Items.Add(item);
                            }

                            DialogResult dialogResult = MessageBox.Show("Would you like to add " + prod.Name + " to Wish List ?", "Insufficient  " + prod.Name, MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                bool existInWishList = false;
                                ListViewItem itemL2;

                                foreach (Models.Product WishProd in wishList)
                                {
                                    if (WishProd.Name.Equals(resp))
                                    {
                                        WishProd.Quantity = WishProd.Quantity + int.Parse(textBox3.Text);
                                        existInWishList = true;
                                    }
                                }

                                if (existInWishList == false)
                                {
                                    Models.Product NewWish = (Models.Product)prod.Clone();
                                    NewWish.Quantity = WishListQuantity;
                                    wishList.Add(NewWish);
                                }

                                listView2.Items.Clear();
                                foreach (Models.Product wishProd in wishList)
                                {
                                    itemL2 = new ListViewItem(new string[] { wishProd.Name, wishProd.Quantity.ToString() });
                                    listView2.Items.Add(itemL2);
                                }

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

        private void button3_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            wishList.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime localDate = DateTime.Today;
            try
            {

                using (System.IO.StreamWriter tw = File.CreateText(@"C:\Users\Iordache Razvan\Documents\Visual Studio 2015\Projects\Licenta\WishList\List " + localDate.ToString("D")+ ".txt"))
                {
                    tw.WriteLine("Produs,Quantity,");
                    foreach (ListViewItem item in listView2.Items)
                    {

                        foreach (ListViewItem.ListViewSubItem listViewSubItem in item.SubItems)
                        {
                            tw.Write(listViewSubItem.Text + ",");
                        }
                        tw.WriteLine();
                    }
                }
                DialogResult dialogResult = MessageBox.Show("Print List " + localDate.ToString("D")+ ".txt","Print Confirmation" , MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ProcessStartInfo psi = new ProcessStartInfo(@"C:\Users\Iordache Razvan\Documents\Visual Studio 2015\Projects\Licenta\WishList\List " + localDate.ToString("D") + ".txt");
                    psi.Verb = "PRINT";

                    Process.Start(psi);
                }
            }
            catch
            {
                MessageBox.Show("TEXT FILE NOT FOUND");
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DateTime localDate = DateTime.Today;
            string WishListNotification = System.IO.File.ReadAllText(@"C:\Users\Iordache Razvan\Documents\Visual Studio 2015\Projects\Licenta\WishList\List " + localDate.ToString("D") + ".txt");

            new PushBullet().SendNotification(new Notifications.Models.Notification()
            {
                Title = "Wish List",
                Body = WishListNotification,
                Type = PushBulletTypes.Note.ToString().ToLower(),
                
            });
        }
    }
}

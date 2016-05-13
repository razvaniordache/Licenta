using Formular.DataSources;
using Formular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Formular
{
    public class Fridge
    {
        protected static List<User> usersTable;
        public static List<Product> availableItems;

        protected string ta = null;

        public void setTa(string ta)
        {
            this.ta = ta;
        }

        public Fridge()
        {
            usersTable = new List<User>();
            availableItems = new List<Product>();
        }

        public Fridge(List<User> _usersTable, List <Product> _availableItems)

        {
            usersTable = _usersTable;
            availableItems = _availableItems;
        }
        public string checkProdus(/*User user, Product p,*/String cod)
        {
            //var userEntry = usersTable.FirstOrDefault(u => u.UserId == user.UserId);
            BDProduseDataSetTableAdapters.ProduseTableAdapter produse = new BDProduseDataSetTableAdapters.ProduseTableAdapter();
            BDProduseDataSet.ProduseDataTable dt = produse.GetDataByCod(cod);

            if(dt.Rows.Count>0)
            {
                return dt.Rows[0][2].ToString();
               
            }
            // if (userentry != null && availableitems.contains(p))
            //{

            //     return productstate.found;
            //}

            // if (products.entries.contains(p.name))
            // {
            //     return productstate.foundinsource;
            // }
            return "Invalid";
        }

        public void LaunchOnlineCommand(string product)
        {
            ProcessStartInfo online = new ProcessStartInfo("http://www.emag.ro/supermarket/search/" + product);
            Process.Start(online);
        }

        public string GetRequest(string barcode)
        {
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create("https://api.outpan.com/v2/products/"+barcode+"?apikey=30ffe6055b8d57a9a80898fdf7dc54e1");
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                string responseJson = sr.ReadToEnd();
                var serializer = new JsonSerializer();
                JObject o = JObject.Parse(responseJson);
                string brand= (string)o["attributes"]["Brand"];
                string Name = (string)o["name"];
                return brand + "-" + Name;
            }
  
            
        }

    }
}


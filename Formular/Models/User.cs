using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formular.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        //public List<Product> Products { get; set; }
        public string UserName { get; set; }
        public User()
        {
            UserId = new Guid();
           // Products = new List<Product>();
            UserName = "Anonim";
        }

    }
}

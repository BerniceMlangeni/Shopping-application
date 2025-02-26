using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Model
{
    public class ShoppingCart
    {
        [PrimaryKey, AutoIncrement]
        public int CartId { get; set; }

        // Foreign Keys
        public int ProfileId { get; set; }
        public int ItemId { get; set; }  
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}

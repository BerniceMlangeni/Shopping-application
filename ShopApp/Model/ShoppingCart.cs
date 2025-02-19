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
        public Guid ProfileId { get; set; } // Links to Profile (Guid)
        public int ItemId { get; set; }      // Links to ShoppingItem (int)
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}

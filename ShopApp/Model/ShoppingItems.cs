using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Model
{
    public class ShoppingItems
    {
        [PrimaryKey, AutoIncrement]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool AddedToCart { get; set; }
        public string ImagePath { get; set; }
      
    }
}

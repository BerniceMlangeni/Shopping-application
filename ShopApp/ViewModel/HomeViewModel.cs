using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Model;
using ShopApp.Helper;
using System.Windows.Input;

namespace ShopApp.ViewModel
{
    public class HomeViewModel
    {
        private readonly DatabaseHelper _databaseHelper;
        public ObservableCollection<ShoppingItems> Products { get; set; }

        public HomeViewModel(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            Products = new ObservableCollection<ShoppingItems>();

            // Load data from database
            LoadShoppingItems();
        }
        
        private async void LoadShoppingItems()
        {
            var items = await _databaseHelper.GetShoppingItemsAsync();
            if (items.Count == 0)
            {
                // Add default items to the database if empty
                var defaultItems = new List<ShoppingItems>
                {
                    new ShoppingItems { ItemId = 1, ItemName = "A little life", Description = "Hanya Yanagihara", Price = 180.00, ImagePath = "little.png", AddedToCart = false },
                    new ShoppingItems { ItemId = 2, ItemName = "48 laws of power", Description = "Robert Greene", Price = 250.00, ImagePath = "power.png", AddedToCart = false },
                    new ShoppingItems { ItemId = 3, ItemName = "To kill a mockingbird", Description = "Harper Lee", Price = 175.00, ImagePath = "bird.png", AddedToCart = false },
                    new ShoppingItems { ItemId = 4, ItemName = "The girl with the louding voice", Description = "Abi Dare", Price = 155.00, ImagePath = "louding.png", AddedToCart = false },
                    new ShoppingItems { ItemId = 5, ItemName = "Atomic habits", Description = "James Clear", Price = 300.00, ImagePath = "habits.png", AddedToCart = false },
                    new ShoppingItems { ItemId = 6, ItemName = "It starts with us", Description = "Colleen Hoover", Price = 170.00, ImagePath = "start.png", AddedToCart = false }
                };

                foreach (var item in defaultItems)
                {
                    await _databaseHelper.SaveShoppingItemAsync(item);
                    Products.Add(item);
                }
            }
            else
            {
                // Load items from database
                foreach (var item in items)
                {
                    Products.Add(item);
                }
            }
        }


    }

}

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using ShopApp.Model;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Helper
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath) 
        {
            //creating tables to accept data  and initialize database connection
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Profile>().Wait();
            _database.CreateTableAsync<ShoppingItems>().Wait();
            _database.CreateTableAsync<ShoppingCart>().Wait();
        }

        public Task<int> SaveProfileAsync(Profile profile) //saves and update the user's profile
        {
            return _database.InsertOrReplaceAsync(profile);
        }

        public Task<Profile> GetProfileAsync() //retrieve the user's profile from the database
        {
            return _database.Table<Profile>().FirstOrDefaultAsync();
        }

        public Task<int> SaveShoppingItemAsync(ShoppingItems item) //saves and updates the shopping items in the database
        {
            return _database.InsertOrReplaceAsync(item);
        }

        public Task<int> DeleteShoppingItemAsync(ShoppingItems item) // deletes a single shopping item
        {
            return _database.DeleteAsync(item);
        }

        public Task<int> DeleteAllShoppingItemsAsync() //deletes all items from your cart
        {
            return _database.DeleteAllAsync<ShoppingItems>();
        }

        public Task<List<ShoppingItems>> GetShoppingItemsAsync() //Retrieves the items from the database
        {
            return _database.Table<ShoppingItems>().ToListAsync();
        }

        public Task<ShoppingItems> GetShoppingItemAsync(int itemId)
        {
            return _database.Table<ShoppingItems>()
                .Where(i => i.ItemId == itemId)
                .FirstOrDefaultAsync();
        }

        public Task<int> UpdateShoppingItemAsync(ShoppingItems item) //updates existing shopping cart
        {
            return _database.UpdateAsync(item);
        }

        public Task<int> SaveCartItemAsync(ShoppingCart cartItem) //adds new item into cart
        {
            return _database.InsertAsync(cartItem);
        }

        public Task<ShoppingCart> GetCartItemAsync(int profileId, int itemId) //retrieves the item by profile and item id
        {
            return _database.Table<ShoppingCart>()
                .Where(c => c.ProfileId == profileId && c.ItemId == itemId)
                .FirstOrDefaultAsync();
        }

        public Task<List<ShoppingCart>> GetCartItemsAsync(int profileId) //retrieves all cart items
        {
            return _database.Table<ShoppingCart>()
                .Where(c => c.ProfileId == profileId)
                .ToListAsync();
        }

        public Task<int> UpdateCartItemAsync(ShoppingCart cartItem) //updates existing cart
        {
            return _database.UpdateAsync(cartItem);
        }

        public Task<int> DeleteCartItemAsync(ShoppingCart cartItem) //deletes an item from cart
        {
            return _database.DeleteAsync(cartItem);
        }

        public Task<int> ClearCartAsync(int profileId) //clears all cart items
        {
            return _database.Table<ShoppingCart>()
                .Where(c => c.ProfileId == profileId)
                .DeleteAsync();
        }

    }
}

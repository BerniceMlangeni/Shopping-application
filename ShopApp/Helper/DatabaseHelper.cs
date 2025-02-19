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

        public DatabaseHelper(string dbPath) // Ensure the constructor accepts dbPath
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Profile>().Wait();
        }

        public Task<int> SaveProfileAsync(Profile profile)
        {
            return _database.InsertOrReplaceAsync(profile);
        }

        public Task<Profile> GetProfileAsync()
        {
            return _database.Table<Profile>().FirstOrDefaultAsync();
        }

        public Task<int> SaveShoppingItemAsync(ShoppingItems item)
        {
            return _database.InsertOrReplaceAsync(item);
        }

        public Task<List<ShoppingItems>> GetShoppingItemsAsync()
        {
            return _database.Table<ShoppingItems>().ToListAsync();
        }

        public Task<int> UpdateShoppingItemAsync(ShoppingItems item)
        {
            return _database.UpdateAsync(item);
        }

    }
}

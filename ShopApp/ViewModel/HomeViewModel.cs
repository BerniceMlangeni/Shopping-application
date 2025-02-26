using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Model;
using ShopApp.Helper;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShopApp.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseHelper _databaseHelper;
        private ObservableCollection<ShoppingItems> _allProducts;
        private ObservableCollection<ShoppingItems> _filteredProducts;
        private string _searchText;
        public ObservableCollection<ShoppingItems> Products
        {
            get => _filteredProducts;
            set
            {
                if (_filteredProducts != value)
                {
                    _filteredProducts = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    FilterProducts();
                }
            }
        }

        public ICommand AddToCartCommand { get; private set; }
        public ICommand ClearSearchCommand { get; private set; }

        public HomeViewModel(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _allProducts = new ObservableCollection<ShoppingItems>();
            _filteredProducts = new ObservableCollection<ShoppingItems>();

            AddToCartCommand = new Command<int>(async (itemId) => await AddToCartAsync(itemId));
            ClearSearchCommand = new Command(() =>
            {
                SearchText = string.Empty;
            });
            // Load data from database
            LoadShoppingItems();
        }

        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) // If search text is empty then show all products
            {
                Products = new ObservableCollection<ShoppingItems>(_allProducts);
                return;
            }

            var searchTerms = SearchText.ToLower().Split(' ');

            var filtered = _allProducts.Where(item =>
                searchTerms.Any(term =>
                    item.ItemName.ToLower().Contains(term) ||
                    item.Description.ToLower().Contains(term)
                )
            ).ToList();

            Products = new ObservableCollection<ShoppingItems>(filtered);
        }

        private async Task AddToCartAsync(int itemId)  // Adds a book to the shopping cart
        {
            // Get the current profile
            var profile = await _databaseHelper.GetProfileAsync();
            if (profile == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please create a profile first!", "OK");
                return; //you have to create a profile first before ading an item to the cart
            }

            // Get the selected item
            var item = _allProducts.FirstOrDefault(i => i.ItemId == itemId);
            if (item == null) return;

            // Check if the item already exists in your cart
            var existingCart = await _databaseHelper.GetCartItemAsync(profile.Id, itemId);

            if (existingCart != null)
            {
                // Update quantity of the item
                existingCart.Quantity += 1;
                await _databaseHelper.UpdateCartItemAsync(existingCart);
            }
            else
            {
                // Add a new item to cart
                var cartItem = new ShoppingCart
                {
                    ProfileId = profile.Id,
                    ItemId = itemId,
                    Quantity = 1,
                    Price = item.Price
                };
                await _databaseHelper.SaveCartItemAsync(cartItem);
            }

            await Application.Current.MainPage.DisplayAlert("Success", $"{item.ItemName} added to cart!", "OK");
        }

        private async void LoadShoppingItems() //Loads shopping items from the database and initializes the Products collection
        {
            var items = await _databaseHelper.GetShoppingItemsAsync();

           
            if (items.Count < 22)
            {
                // Clear existing items first
                foreach (var item in items)
                {
                    await _databaseHelper.DeleteShoppingItemAsync(item);
                }

                // Add all 22 items to the database
                var defaultItems = new List<ShoppingItems>
        {
            new ShoppingItems { ItemId = 1, ItemName = "A Little Life", Description = "Hanya Yanagihara", Price = 180.00, ImagePath = "little.png", AddedToCart = false },
            new ShoppingItems { ItemId = 2, ItemName = "48 Laws Of Power", Description = "Robert Greene", Price = 250.00, ImagePath = "power.png", AddedToCart = false },
            new ShoppingItems { ItemId = 3, ItemName = "To Kill A Mockingbird", Description = "Harper Lee", Price = 175.00, ImagePath = "bird.png", AddedToCart = false },
            new ShoppingItems { ItemId = 4, ItemName = "The Girl With The Louding Voice", Description = "Abi Dare", Price = 155.00, ImagePath = "louding.png", AddedToCart = false },
            new ShoppingItems { ItemId = 5, ItemName = "Atomic Habits", Description = "James Clear", Price = 300.00, ImagePath = "habits.png", AddedToCart = false },
            new ShoppingItems { ItemId = 6, ItemName = "It Starts With Us", Description = "Colleen Hoover", Price = 170.00, ImagePath = "start.png", AddedToCart = false },
            new ShoppingItems { ItemId = 7, ItemName = "If Only I Had Told Her", Description = "Laura Nowlin", Price = 235.00, ImagePath = "laura.png", AddedToCart = false },
            new ShoppingItems { ItemId = 8, ItemName = "Fourth Wing", Description = "Rebecca Yarros", Price = 230.00, ImagePath = "wing.png", AddedToCart = false },
            new ShoppingItems { ItemId = 9, ItemName = "Book Lovers", Description = "Emily Henry", Price = 200.00, ImagePath = "booklovers.png", AddedToCart = false },
            new ShoppingItems { ItemId = 10, ItemName = "If He Had Been With Me", Description = "Colleen Hoover", Price = 165.00, ImagePath = "laura2.png", AddedToCart = false },
            new ShoppingItems { ItemId = 11, ItemName = "Icebreaker", Description = "Hannah Grace", Price = 163.00, ImagePath = "ice.png", AddedToCart = false },
            new ShoppingItems { ItemId = 12, ItemName = "12 Rules For Life", Description = "Jordan B Peterson", Price = 165.00, ImagePath = "rules.png", AddedToCart = false },
            new ShoppingItems { ItemId = 13, ItemName = "Diary Of a CEO", Description = "Steven Bartlett", Price = 251.00, ImagePath = "diary.png", AddedToCart = false },
            new ShoppingItems { ItemId = 14, ItemName = "Every Last Word", Description = "Tamara Ireland Stone", Price = 200.00, ImagePath = "word.png", AddedToCart = false },
            new ShoppingItems { ItemId = 15, ItemName = "Dear Self", Description = "Patience Tamara Davis", Price = 232.00, ImagePath = "self.png", AddedToCart = false },
            new ShoppingItems { ItemId = 16, ItemName = "Ego is The Enemy", Description = "Ryan Holiday", Price = 201.00, ImagePath = "ego.png", AddedToCart = false },
            new ShoppingItems { ItemId = 17, ItemName = "The Mountain Is You", Description = "Brianna Wiest", Price = 449.00, ImagePath = "mountain.png", AddedToCart = false },
            new ShoppingItems { ItemId = 18, ItemName = "How to Heal", Description = "Thich Nnat Hanh", Price = 135.00, ImagePath = "heal.png", AddedToCart = false },
            new ShoppingItems { ItemId = 19, ItemName = "Six of Crows", Description = "Leigh Bardugo", Price = 155.00, ImagePath = "six.png", AddedToCart = false },
            new ShoppingItems { ItemId = 20, ItemName = "The Silent Patient", Description = "Alex Michaelides", Price = 170.00, ImagePath = "patient.png", AddedToCart = false },
            new ShoppingItems { ItemId = 21, ItemName = "Binding 13", Description = "Chloe Walsh", Price = 190.00, ImagePath = "binding.png", AddedToCart = false },
            new ShoppingItems { ItemId = 22, ItemName = "The Housemaid", Description = "Freida McFadden", Price = 199.00, ImagePath = "housemaid.png", AddedToCart = false }
        };
                // Save each item to database and add to collection
                foreach (var item in defaultItems)
                {
                    await _databaseHelper.SaveShoppingItemAsync(item);
                    _allProducts.Add(item);
                }
            }
            else
            {
                foreach (var item in items)
                {
                    _allProducts.Add(item);
                }
            }

            // Initialize filtered products with all products
            Products = new ObservableCollection<ShoppingItems>(_allProducts);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}



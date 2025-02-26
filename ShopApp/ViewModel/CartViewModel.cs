using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ShopApp.Helper;
using ShopApp.Model;

namespace ShopApp.ViewModel
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseHelper _databaseHelper;
        private Profile _currentProfile;
        private double _totalAmount;

        public ObservableCollection<CartItemViewModel> CartItems { get; set; }  // Collection of items in the user's cart
        public ICommand IncreaseQuantityCommand { get; private set; }
        public ICommand DecreaseQuantityCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        public ICommand CheckoutCommand { get; private set; }

        public double TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public CartViewModel(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            CartItems = new ObservableCollection<CartItemViewModel>();

            // Initialize commands
            IncreaseQuantityCommand = new Command<int>(async (itemId) => await UpdateQuantityAsync(itemId, 1));
            DecreaseQuantityCommand = new Command<int>(async (itemId) => await UpdateQuantityAsync(itemId, -1));
            RemoveItemCommand = new Command<int>(async (itemId) => await RemoveItemAsync(itemId));
            CheckoutCommand = new Command(async () => await CheckoutAsync());

            LoadCartItemsAsync();
        }

        private async void LoadCartItemsAsync()
        {
            try
            {
                _currentProfile = await _databaseHelper.GetProfileAsync();
                if (_currentProfile == null) return;
                CartItems.Clear();

                // Get cart items for this profile
                var cartItems = await _databaseHelper.GetCartItemsAsync(_currentProfile.Id);
                foreach (var cartItem in cartItems)
                {
                    // Get the product details
                    var product = await _databaseHelper.GetShoppingItemAsync(cartItem.ItemId);
                    if (product != null)
                    {
                        CartItems.Add(new CartItemViewModel
                        {
                            CartId = cartItem.CartId,
                            ItemId = cartItem.ItemId,
                            ProfileId = cartItem.ProfileId,
                            ItemName = product.ItemName,
                            ImagePath = product.ImagePath,
                            Price = product.Price,
                            Quantity = cartItem.Quantity,
                            TotalPrice = product.Price * cartItem.Quantity
                        });
                    }
                }

                CalculateTotalAmount();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load cart items: {ex.Message}", "OK");
            }
        }

        private async Task UpdateQuantityAsync(int itemId, int change)
        {
            var cartItemVM = CartItems.FirstOrDefault(ci => ci.ItemId == itemId);
            if (cartItemVM == null) return;

            var cartItem = await _databaseHelper.GetCartItemAsync(_currentProfile.Id, itemId);
            if (cartItem == null) return;

            // Update quantity (minimum of 1)
            cartItem.Quantity = Math.Max(1, cartItem.Quantity + change);
            await _databaseHelper.UpdateCartItemAsync(cartItem);

            cartItemVM.Quantity = cartItem.Quantity;
            cartItemVM.TotalPrice = cartItemVM.Price * cartItem.Quantity;

            CalculateTotalAmount();
        }

        private async Task RemoveItemAsync(int itemId)
        {
            var cartItemVM = CartItems.FirstOrDefault(ci => ci.ItemId == itemId);
            if (cartItemVM == null) return;

            var cartItem = await _databaseHelper.GetCartItemAsync(_currentProfile.Id, itemId);
            if (cartItem == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Remove Item",
                $"Are you sure you want to remove {cartItemVM.ItemName} from your cart?",
                "Yes", "No");

            if (!confirm) return;

            await _databaseHelper.DeleteCartItemAsync(cartItem);

            CartItems.Remove(cartItemVM);

            CalculateTotalAmount();
        }

        private async Task CheckoutAsync()
        {
            if (CartItems.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Cart Empty", "Your cart is empty.", "OK");
                return;
            }

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Checkout",
                $"Proceed to checkout? Total: R{TotalAmount:F2}",
                "Yes", "No");

            if (!confirm) return; 

            // Clear cart after successful checkout
            await _databaseHelper.ClearCartAsync(_currentProfile.Id);
            CartItems.Clear();
            TotalAmount = 0;

            await Application.Current.MainPage.DisplayAlert("Success", "Your order has been placed!", "OK");
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = CartItems.Sum(item => item.TotalPrice);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Helper class for cart item display
    public class CartItemViewModel : INotifyPropertyChanged
    {
        private int _quantity;
        private double _totalPrice;

        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int ProfileId { get; set; } 
        public string ItemName { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    TotalPrice = Price * _quantity;
                }
            }
        }

        public double TotalPrice
        {
            get => _totalPrice;
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
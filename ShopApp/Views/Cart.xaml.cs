using ShopApp.Helper;
using ShopApp.Model;
using ShopApp.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace ShopApp.Views;

public partial class Cart : ContentPage
{
    private CartViewModel _viewModel;
    public Cart()
	{
		InitializeComponent();
        _viewModel = new CartViewModel(App.Database);
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh the cart when the page appears
        _viewModel = new CartViewModel(App.Database);
        BindingContext = _viewModel;
    }

}
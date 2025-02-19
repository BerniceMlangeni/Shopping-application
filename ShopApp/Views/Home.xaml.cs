using ShopApp.ViewModel;
namespace ShopApp.Views;

public partial class Home : ContentPage
{
	public Home()
	{
		InitializeComponent();
        BindingContext = new HomeViewModel(App.Database);
    }
}
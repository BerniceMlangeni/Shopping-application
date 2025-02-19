using ShopApp.Views;

namespace ShopApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Home", typeof(Home));
            Routing.RegisterRoute("Cart", typeof(Cart));
            Routing.RegisterRoute("profilePage", typeof(ProfilePage));
        }
    }
}

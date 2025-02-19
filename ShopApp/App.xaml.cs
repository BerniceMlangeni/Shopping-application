using ShopApp.Helper;
namespace ShopApp
{
    public partial class App : Application
    {
        private static DatabaseHelper _databaseHelper;
        public static DatabaseHelper Database => _databaseHelper ??= new DatabaseHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "profiles.db"));

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}

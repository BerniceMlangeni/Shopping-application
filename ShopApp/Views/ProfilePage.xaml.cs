using ShopApp.Helper;
using ShopApp.Model;
namespace ShopApp.Views;

public partial class ProfilePage : ContentPage
{
    private readonly DatabaseHelper _database;
    private string profileImagePath;
    public ProfilePage()
	{
		InitializeComponent();
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "profiles.db");
        _database = new DatabaseHelper(dbPath);
        LoadProfile();
    }

    private async void LoadProfile()
    {
        var profile = await _database.GetProfileAsync();
        if (profile != null)
        {
            NameEntry.Text = profile.Name;
            SurnameEntry.Text = profile.Surname;
            EmailEntry.Text = profile.Email;
            BioEntry.Text = profile.Bio;
            profileImagePath = profile.ProfilePicture;

            if (!string.IsNullOrEmpty(profileImagePath) && File.Exists(profileImagePath))
            {
                ProfileImage.Source = ImageSource.FromFile(profileImagePath);
            }
        }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var profile = new Profile
        {
            Id = 1, // Always use the same ID so there's only one profile stored
            Name = NameEntry.Text,
            Surname = SurnameEntry.Text,
            Email = EmailEntry.Text,
            Bio = BioEntry.Text,
            ProfilePicture = profileImagePath
        };

        await _database.SaveProfileAsync(profile);
        await DisplayAlert("Success", "Profile saved successfully!", "OK");
        ViewDataButton.IsVisible = true;
    }

    private async void OnAddProfilePictureClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images });
        if (result != null)
        {
            profileImagePath = result.FullPath;
            ProfileImage.Source = ImageSource.FromFile(profileImagePath);
        }
    }

    private async void OnViewDataClicked(object sender, EventArgs e)
    {
        var profile = await _database.GetProfileAsync();
        if (profile != null)
        {
            await DisplayAlert("Profile Data",
                $"Name: {profile.Name}\nSurname: {profile.Surname}\nEmail: {profile.Email}\nBio: {profile.Bio}",
                "OK");
        }
        else
        {
            await DisplayAlert("Error", "No profile found in the database.", "OK");
        }
    }

}
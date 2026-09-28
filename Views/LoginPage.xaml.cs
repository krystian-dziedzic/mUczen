using mUczen.Services;
using mUczen.Models;
namespace mUczen.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private async void Animate_Logo(object sender, EventArgs e)
        {
            while (true)
            {
                await Logo.ScaleToAsync(1.15, 800);
                await Logo.ScaleToAsync(1.0, 800);
            }
        }
        private async void DisplayLoginPopup(object sender, EventArgs e)
        {
            LoginPopup.TranslationX = Width;
            LoginPopup.IsVisible = true;

            await LoginPopup.TranslateToAsync(0, 0, 400);
        }

        private void LoginButtonClicked(object sender, EventArgs e)
        {
            mUczen.Services.Database.Connection.Insert(new User 
            { 
                FirstName = "Adam",
                IsAdmin = true,
                Password = mUczen.Services.Database.hasher.HashPassword(PasswordEntry.Text),
                StudentIdCard = "1" 
            });
            LoginPopup.IsVisible = false;
        }
    }
}

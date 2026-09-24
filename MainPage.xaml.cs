using mUczen.Models;
namespace mUczen
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
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
        private void DisplayLoginPopup(object sender, EventArgs e)
        {
            LoginPopup.IsVisible = true;
        }

        private void LoginButtonClicked(object sender, EventArgs e)
        {
            User testUser = new User
            {
                FirstName = "John",
                Email = "JohnPork@gmail.com",
                Password = "John_Pork_Password_1"
            };
            LoginPopup.IsVisible = false;
        }
    }
}

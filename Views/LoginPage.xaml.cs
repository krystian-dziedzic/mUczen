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
        private void DisplayLoginPopup(object sender, EventArgs e)
        {
            LoginPopup.IsVisible = true;
        }

        private void LoginButtonClicked(object sender, EventArgs e)
        {
            // Logika logowania użytkownika (do bazy danych)
            LoginPopup.IsVisible = false;
        }
    }
}

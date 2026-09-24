using mUczen.Services;

namespace mUczen
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Services.Database.Initialize();
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
            LoginPopup.IsVisible = false;
        }
    }
}

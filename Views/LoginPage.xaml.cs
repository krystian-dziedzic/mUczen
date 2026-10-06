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

        //

        //private async void DisplayLoginPopup(object sender, EventArgs e)
        //{
        //    LoginPopup.TranslationX = Width;
        //    LoginPopup.IsVisible = true;
        //    LoginButton.IsEnabled = false;
        //    RegisterButton.IsEnabled = false;

        //    await LoginPopup.TranslateToAsync(0, 0, 400);
        //}

        private async void DisplayRegisterPopup(object sender, EventArgs e)
        {
            RegisterPopup.TranslationX = Width;
            RegisterPopup.IsVisible = true;
            LoginButton.IsEnabled = false;
            RegisterButton.IsEnabled = false;

            await RegisterPopup.TranslateToAsync(0, 0, 400);
        }

        //private async void DisplayAccountCreationPopup(object sender, EventArgs e)
        //{
        //
        //}

        //private void LoginButtonClicked(object sender, EventArgs e)
        //{
        //    // Logika logowania użytkownika (do bazy danych)
        //    LoginPopup.IsVisible = false;

        //    // TEMP BUTTON ENABLING
        //    LoginButton.IsEnabled = true;
        //    RegisterButton.IsEnabled = true;
        //}

        private void RegisterButtonClicked(object sender, EventArgs e)
        {
            // Logika rejestrowania użytkownika (do bazy danych)
            RegisterPopup.IsVisible = false;

            // TEMP BUTTON ENABLING
            LoginButton.IsEnabled = true;
            RegisterButton.IsEnabled = true;
        }
    }
}

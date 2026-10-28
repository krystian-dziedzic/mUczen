namespace mUczen.Views
{
    public partial class LoginPage : ContentPage
    {
        private int _logoAnimationId = 0;
        private bool _isAnimatingPopup = false;

        public LoginPage()
        {
            InitializeComponent();

            TopSection.Opacity = 0;
            BottomSection.Opacity = 0;
        }

        // ========================= ANIMACJE =========================

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            TopSection.TranslationY = -20;
            BottomSection.TranslationY = 30;

            await Task.WhenAll(
                TopSection.FadeToAsync(1, 600, Easing.CubicOut),
                TopSection.TranslateToAsync(0, 0, 600, Easing.CubicOut),
                BottomSection.FadeToAsync(1, 700, Easing.CubicOut),
                BottomSection.TranslateToAsync(0, 0, 700, Easing.CubicOut));
        }

        private async void Animate_Logo(object sender, EventArgs e)
        {
            int myId = ++_logoAnimationId;
            while (myId == _logoAnimationId)
            {
                await Logo.ScaleToAsync(1.12, 1000, Easing.SinInOut);
                if (myId != _logoAnimationId) break;
                await Logo.ScaleToAsync(1.0, 1000, Easing.SinInOut);
            }
        }

        private void Stop_Logo(object sender, EventArgs e)
        {
            _logoAnimationId++;
        }

        // ========================= POPUPY =========================

        private async Task ShowPopup(VisualElement popup)
        {
            if (_isAnimatingPopup) return;
            _isAnimatingPopup = true;

            popup.TranslationX = Width;
            popup.IsVisible = true;
            await popup.TranslateToAsync(0, 0, 350, Easing.CubicOut);

            _isAnimatingPopup = false;
        }

        private async Task HidePopup(VisualElement popup)
        {
            if (_isAnimatingPopup) return;
            _isAnimatingPopup = true;

            await popup.TranslateToAsync(Width, 0, 300, Easing.CubicIn);
            popup.IsVisible = false;

            _isAnimatingPopup = false;
        }

        private async void DisplayLoginPopup(object sender, EventArgs e)
        {
            await ShowPopup(LoginPopup);
        }

        private async void DisplayRegisterPopup(object sender, EventArgs e)
        {
            await ShowPopup(RegisterPopup);
        }

        private async void CloseLoginPopup(object sender, EventArgs e)
        {
            await HidePopup(LoginPopup);
        }

        private async void CloseRegisterPopup(object sender, EventArgs e)
        {
            await HidePopup(RegisterPopup);
        }

        private async void SwitchToRegister(object sender, TappedEventArgs e)
        {
            await ShowPopup(RegisterPopup);
            LoginPopup.IsVisible = false;
        }

        private async void SwitchToLogin(object sender, TappedEventArgs e)
        {
            LoginPopup.TranslationX = 0;
            LoginPopup.IsVisible = true;
            await HidePopup(RegisterPopup);
        }

        protected override bool OnBackButtonPressed()
        {
            if (RegisterPopup.IsVisible)
            {
                CloseRegisterPopup(this, EventArgs.Empty);
                return true;
            }
            if (LoginPopup.IsVisible)
            {
                CloseLoginPopup(this, EventArgs.Empty);
                return true;
            }
            return base.OnBackButtonPressed();
        }

        // ========================= POLA FORMULARZY =========================

        private void TogglePasswordVisibility(object sender, TappedEventArgs e)
        {
            if (sender is not Label label) return;

            Entry entry = label == LoginShowPasswordLabel ? PasswordEntry : CreateAccountPasswordEntry;

            entry.IsPassword = !entry.IsPassword;
            label.Text = entry.IsPassword ? "Pokaż" : "Ukryj";
        }

        private void EntryCompleted(object sender, EventArgs e)
        {
            if (sender == StudentIdCardEntry)
                PasswordEntry.Focus();
            else if (sender == PasswordEntry)
                LoginButtonClicked(sender, e);
            else if (sender == CreateAccountFirstName)
                CreateAccountLastName.Focus();
            else if (sender == CreateAccountLastName)
                CreateAccountStudentIdCardEntry.Focus();
            else if (sender == CreateAccountStudentIdCardEntry)
                CreateAccountPasswordEntry.Focus();
            else if (sender == CreateAccountPasswordEntry)
                CreateAccountButtonClicked(sender, e);
        }

        // ========================= LOGIKA =========================

        private void CreateAccountButtonClicked(object sender, EventArgs e)
        {
            // REJESTRACJA
        }

        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StudentIdCardEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlertAsync("Błąd", "Proszę wypełnić wszystkie pola.", "OK");
                return;
            }
        }
    }
}
using mUczen.Services;
using mUczen.Models;
using SQLite;
using Microsoft.AspNetCore.Identity;
namespace mUczen.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Database.Initialize();
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

        private bool LoginCheck(string studentIdCard, string password)
        {
            var user = Database.Connection.Query<User>("SELECT * FROM User WHERE StudentIdCard = ?", studentIdCard);
            if (user.Count == 0)
            {
                return false;
            }
            else
            {
                if (Database.hasher.VerifyHashedPassword(user[0], user[0].Password, password) == PasswordVerificationResult.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //WPROWADZIĆ --LOCAL STORAGE-- I ZABEZPIECZENIA TOKENÓW
        private void CreateAccountButtonClicked(object sender, EventArgs e)
        {
            //Test user
            Database.Connection.Insert(new User
            {
                StudentIdCard = "00000001",
                Password = Database.hasher.HashPassword(new User(), "testpassword"),
                FirstName = "Władysław",
                IsAdmin = true
            });
        }
        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StudentIdCardEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlertAsync("Błąd", "Proszę wypełnić wszystkie pola.", "OK");
                return;
            }
            else if (StudentIdCardEntry.Text.Length != 8)
            {
                await DisplayAlertAsync("Błąd", "Numer legitymacji musi mieć 8 znaków.", "OK");
                return;
            }
            else if (PasswordEntry.Text.Length < 6)
            {
                await DisplayAlertAsync("Błąd", "Hasło musi mieć minimum 6 znaków.", "OK");
                return;
            }
            else if (LoginCheck(StudentIdCardEntry.Text, PasswordEntry.Text))
            {
                await DisplayAlertAsync("Sukces", "Zalogowano pomyślnie!", "OK");
                //await Shell.Current.GoToAsync("//MainPage");
                //nawigacja do strony głównej po zalogowaniu
            }
            else
            {
                await DisplayAlertAsync("Błąd", "Nieprawidłowy numer legitymacji lub hasło.", "OK");
            }
        }
    }
}

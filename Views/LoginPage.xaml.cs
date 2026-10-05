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

        //private bool AccountCreationCheck(string password, string firstName, string studentidcard, string lastName)
        //{
        //    if (string.IsNullOrEmpty(password) ||
        //        string.IsNullOrEmpty(firstName) ||
        //        string.IsNullOrEmpty(studentidcard) ||
        //        string.IsNullOrEmpty(lastName))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (password.Length < 8)
        //        {
        //            return false;
        //        }
        //    }
        //}
        //WPROWADZIĆ --LOCAL STORAGE-- I ZABEZPIECZENIA TOKENÓW
        private void CreateAccountButtonClicked(object sender, EventArgs e)
        {
            
        }
        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StudentIdCardEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlertAsync("Błąd", "Proszę wypełnić wszystkie pola.", "OK");
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

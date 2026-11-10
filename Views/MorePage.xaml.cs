using mUczen.Services;

namespace mUczen.Views;

public partial class MorePage : ContentPage
{
    public MorePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Session.CurrentUser is { } user)
            AccountInfoLabel.Text = $"Zalogowano jako {user.FirstName} {user.LastName}";
    }

    private async void LogoutClicked(object sender, EventArgs e)
    {
        Session.CurrentUser = null;
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
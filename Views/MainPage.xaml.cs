using mUczen.Services;

namespace mUczen.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Session.CurrentUser is { } user)
        {
            UserFirstName.Text = $"{user.FirstName}!";
        }
    }
}
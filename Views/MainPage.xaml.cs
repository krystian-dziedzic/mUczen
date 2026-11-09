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

    private async void OpenGrades(object sender, TappedEventArgs e) => await Navigation.PushAsync(new PlaceholderPage("Oceny"));
    private async void OpenAttendance(object sender, TappedEventArgs e) => await Navigation.PushAsync(new PlaceholderPage("Frekwencja"));
    private async void OpenSchedule(object sender, TappedEventArgs e) => await Navigation.PushAsync(new PlaceholderPage("Plan lekcji"));
    private async void OpenHomework(object sender, TappedEventArgs e) => await Navigation.PushAsync(new PlaceholderPage("Zadania"));
    private async void OpenAnnouncements(object sender, TappedEventArgs e) => await Navigation.PushAsync(new PlaceholderPage("Komunikaty"));
    private async void OpenMore(object sender, TappedEventArgs e) => await Navigation.PushAsync(new PlaceholderPage("Więcej"));
}
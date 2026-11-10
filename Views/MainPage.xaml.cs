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
            UserClassName.Text = user.ClassName;
            CardFullNameLabel.Text = $"{user.FirstName} {user.LastName}";
            CardSchoolLabel.Text = user.SchoolName;
            CardStudentIdLabel.Text = user.StudentIdCard;
        }
    }
    private async void OpenGrades(object sender, TappedEventArgs e) => await Navigation.PushAsync(new GradesPage());
    private async void OpenAttendance(object sender, TappedEventArgs e) => await Navigation.PushAsync(new AttendancePage());
    private async void OpenSchedule(object sender, TappedEventArgs e) => await Navigation.PushAsync(new SchedulePage());
    private async void OpenHomework(object sender, TappedEventArgs e) => await Navigation.PushAsync(new HomeworkPage());
    private async void OpenAnnouncements(object sender, TappedEventArgs e) => await Navigation.PushAsync(new AnnouncementsPage());
    private async void OpenMore(object sender, TappedEventArgs e) => await Navigation.PushAsync(new MorePage());
}
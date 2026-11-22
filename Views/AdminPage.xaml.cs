using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public partial class AdminPage : ContentPage
{
    public AdminPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Session.CurrentUser is not { IsAdmin: true } user)
        {
            await Navigation.PopAsync();
            return;
        }

        AdminNameLabel.Text = $"{user.FirstName} {user.LastName}";
        UsersCountLabel.Text = Database.Connection.Table<User>().Count().ToString();
        SubjectsCountLabel.Text = Database.Connection.Table<ClassSubject>().Count().ToString();
        GradesCountLabel.Text = Database.Connection.Table<Grade>().Count().ToString();
        LessonsCountLabel.Text = Database.Connection.Table<Lesson>().Count().ToString();
        AttendanceCountLabel.Text = Database.Connection.Table<Attendance>().Count().ToString();
    }

    private async void OpenUsers(object sender, TappedEventArgs e) => await Navigation.PushAsync(new AdminUsersPage());
    private async void OpenSubjects(object sender, TappedEventArgs e) => await Navigation.PushAsync(new AdminSubjectsPage());
    private async void OpenGrades(object sender, TappedEventArgs e) => await Navigation.PushAsync(new AdminGradesPage());
    private async void OpenLessons(object sender, TappedEventArgs e) => await Navigation.PushAsync(new AdminLessonsPage());
    private async void OpenAttendance(object sender, TappedEventArgs e) => await Navigation.PushAsync(new AdminAttendancePage());
}

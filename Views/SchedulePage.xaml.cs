using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public partial class SchedulePage : ContentPage
{
    private static readonly (DayOfWeek Day, string Label)[] Days =
    {
        (DayOfWeek.Monday, "Pon"),
        (DayOfWeek.Tuesday, "Wt"),
        (DayOfWeek.Wednesday, "Śr"),
        (DayOfWeek.Thursday, "Czw"),
        (DayOfWeek.Friday, "Pt")
    };

    private DayOfWeek _selectedDay = DateTime.Today.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
        ? DayOfWeek.Monday
        : DateTime.Today.DayOfWeek;

    public SchedulePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BuildDayTabs();
        ReloadLessons();
    }

    private void BuildDayTabs()
    {
        DayTabs.Children.Clear();

        foreach (var (day, label) in Days)
        {
            var isSelected = day == _selectedDay;

            var button = new Button
            {
                Text = label,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 10,
                WidthRequest = 50,
                HeightRequest = 40,
                BackgroundColor = isSelected ? (Color)Application.Current!.Resources["Cerulean"] : (Color)Application.Current!.Resources["Porcelain"],
                TextColor = isSelected ? Colors.White : (Color)Application.Current!.Resources["CharcoalBrown"]
            };
            button.Clicked += (_, _) =>
            {
                _selectedDay = day;
                BuildDayTabs();
                ReloadLessons();
            };

            DayTabs.Children.Add(button);
        }
    }

    private void ReloadLessons()
    {
        LessonsList.Children.Clear();

        if (Session.CurrentUser is not { } user)
            return;

        var lessons = Database.Connection.Table<Lesson>()
            .Where(l => l.DayOfWeek == _selectedDay && l.SchoolName == user.SchoolName && l.ClassName == user.ClassName)
            .ToList()
            .OrderBy(l => l.Duration)
            .ToList();

        foreach (var lesson in lessons)
        {
            var timeLabel = new Label { Text = lesson.Duration.ToString(@"hh\:mm"), FontAttributes = FontAttributes.Bold, WidthRequest = 60, VerticalOptions = LayoutOptions.Center };
            var subjectLabel = new Label { Text = lesson.Subject, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center };
            var teacherLabel = new Label { Text = lesson.Teacher, FontSize = 12, Opacity = 0.65, VerticalOptions = LayoutOptions.Center };
            var infoColumn = new VerticalStackLayout { Spacing = 2, Children = { subjectLabel, teacherLabel } };

            var row = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                ColumnSpacing = 12
            };
            row.Add(timeLabel, 0);
            row.Add(infoColumn, 1);

            LessonsList.Children.Add(new Border
            {
                Padding = 16,
                Stroke = (Color)Application.Current!.Resources["CharcoalBrown"],
                BackgroundColor = (Color)Application.Current!.Resources["Porcelain"],
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
                Content = row
            });
        }

        if (lessons.Count == 0)
            LessonsList.Children.Add(new Label { Text = "Brak lekcji tego dnia.", Opacity = 0.6, HorizontalOptions = LayoutOptions.Center });
    }
}
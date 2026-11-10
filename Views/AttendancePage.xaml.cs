using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public partial class AttendancePage : ContentPage
{
    public AttendancePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Reload();
    }

    private void Reload()
    {
        AttendanceList.Children.Clear();

        if (Session.CurrentUser is not { } user)
            return;

        var records = Database.Connection.Table<Attendance>()
            .Where(a => a.UserId == user.Id)
            .ToList()
            .OrderByDescending(a => a.Date)
            .ToList();

        var lessons = Database.Connection.Table<Lesson>().ToList()
            .ToDictionary(l => l.Id, l => l.Subject ?? "Lekcja");

        foreach (var record in records)
        {
            var subject = lessons.TryGetValue(record.LessonId, out var name) ? name : "Lekcja";
            var statusColor = record.IsPresent ? Color.FromArgb("#2A9D8F") : Color.FromArgb("#E63946");
            var statusText = record.IsPresent ? "Obecny" : "Nieobecny";

            var subjectLabel = new Label { Text = subject, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center };
            var dateLabel = new Label { Text = record.Date.ToString("dd.MM.yyyy"), FontSize = 12, Opacity = 0.65, VerticalOptions = LayoutOptions.Center };
            var infoColumn = new VerticalStackLayout { Spacing = 2, Children = { subjectLabel, dateLabel } };

            var statusLabel = new Label { Text = statusText, TextColor = statusColor, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.End };

            var row = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };
            row.Add(infoColumn, 0);
            row.Add(statusLabel, 1);

            AttendanceList.Children.Add(new Border
            {
                Padding = 16,
                Stroke = (Color)Application.Current!.Resources["CharcoalBrown"],
                BackgroundColor = (Color)Application.Current!.Resources["Porcelain"],
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
                Content = row
            });
        }

        if (records.Count == 0)
            AttendanceList.Children.Add(new Label { Text = "Brak zapisanej frekwencji.", Opacity = 0.6, HorizontalOptions = LayoutOptions.Center });

        AttendancePercentLabel.Text = records.Count > 0
            ? $"{Math.Round(records.Count(r => r.IsPresent) * 100.0 / records.Count)}%"
            : "-";
    }
}
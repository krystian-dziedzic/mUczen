using Microsoft.Maui.Controls.Shapes;
using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public partial class GradesPage : ContentPage
{
    private static readonly string[] Palette =
        { "#2E86AB", "#E63946", "#2A9D8F", "#8E44AD", "#F4A261", "#457B9D" };

    public GradesPage()
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
        GradesList.Children.Clear();

        if (Session.CurrentUser is not { } user)
            return;

        var grades = Database.Connection.Table<Grade>()
            .Where(g => g.UserId == user.Id)
            .ToList();

        var bySubject = grades
            .Where(g => g.Subject != null)
            .GroupBy(g => g.Subject!)
            .Select(group => (Subject: group.Key, Average: Math.Round(group.Average(g => g.Value), 2)))
            .ToList();

        for (int i = 0; i < bySubject.Count; i++)
        {
            var (subject, average) = bySubject[i];
            var color = Color.FromArgb(Palette[i % Palette.Length]);

            var dot = new Ellipse
            {
                WidthRequest = 14,
                HeightRequest = 14,
                Fill = new SolidColorBrush(color),
                VerticalOptions = LayoutOptions.Center
            };

            var subjectLabel = new Label
            {
                Text = subject,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center
            };

            var averageLabel = new Label
            {
                Text = average.ToString("0.00"),
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            };

            var row = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                },
                ColumnSpacing = 12
            };
            row.Add(dot, 0);
            row.Add(subjectLabel, 1);
            row.Add(averageLabel, 2);

            var card = new Border
            {
                Padding = 16,
                Stroke = (Color)Application.Current!.Resources["CharcoalBrown"],
                BackgroundColor = (Color)Application.Current!.Resources["Porcelain"],
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
                Content = row
            };

            GradesList.Children.Add(card);
        }

        if (bySubject.Count == 0)
        {
            GradesList.Children.Add(new Label
            {
                Text = "Brak ocen.",
                Opacity = 0.6,
                HorizontalOptions = LayoutOptions.Center
            });
        }

        AverageLabel.Text = bySubject.Count > 0
            ? Math.Round(bySubject.Average(s => s.Average), 2).ToString("0.00")
            : "-";
    }
}
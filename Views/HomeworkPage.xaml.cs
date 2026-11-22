using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public partial class HomeworkPage : ContentPage
{
    public HomeworkPage()
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
        HomeworkList.Children.Clear();

        if (Session.CurrentUser is not { } user)
            return;

        var homework = Database.Connection.Table<Homework>()
            .Where(h => h.SchoolName == user.SchoolName && h.ClassName == user.ClassName)
            .ToList()
            .OrderBy(h => h.DueDate)
            .ToList();

        foreach (var item in homework)
        {
            var titleLabel = new Label { Text = item.Title, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center };
            var subjectLabel = new Label { Text = item.Subject, FontSize = 12, Opacity = 0.65, VerticalOptions = LayoutOptions.Center };
            var infoColumn = new VerticalStackLayout { Spacing = 2, Children = { titleLabel, subjectLabel } };

            var isOverdue = item.DueDate.Date < DateTime.Today;
            var dueLabel = new Label
            {
                Text = item.DueDate.ToString("dd.MM"),
                FontAttributes = FontAttributes.Bold,
                TextColor = isOverdue ? Color.FromArgb("#E63946") : (Color)Application.Current!.Resources["CharcoalBrown"],
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            };

            var row = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };
            row.Add(infoColumn, 0);
            row.Add(dueLabel, 1);

            HomeworkList.Children.Add(new Border
            {
                Padding = 16,
                Stroke = (Color)Application.Current!.Resources["CharcoalBrown"],
                BackgroundColor = (Color)Application.Current!.Resources["Porcelain"],
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
                Content = row
            });
        }

        if (homework.Count == 0)
            HomeworkList.Children.Add(new Label { Text = "Brak zadań.", Opacity = 0.6, HorizontalOptions = LayoutOptions.Center });
    }
}
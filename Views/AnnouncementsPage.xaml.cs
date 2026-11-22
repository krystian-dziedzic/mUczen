using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public partial class AnnouncementsPage : ContentPage
{
    private static readonly (AnnouncementCategory? Category, string Label)[] Filters =
    {
        (null, "Wszystkie"),
        (AnnouncementCategory.Szkola, "Szkoła"),
        (AnnouncementCategory.Klasa, "Klasa"),
        (AnnouncementCategory.Nauczyciele, "Nauczyciele")
    };

    private AnnouncementCategory? _selectedFilter = null;

    public AnnouncementsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BuildCategoryTabs();
        Reload();
    }

    private void BuildCategoryTabs()
    {
        CategoryTabs.Children.Clear();

        foreach (var (category, label) in Filters)
        {
            var isSelected = category == _selectedFilter;

            var button = new Button
            {
                Text = label,
                FontSize = 13,
                CornerRadius = 10,
                HeightRequest = 36,
                Padding = new Thickness(12, 0),
                BackgroundColor = isSelected ? (Color)Application.Current!.Resources["Cerulean"] : (Color)Application.Current!.Resources["Porcelain"],
                TextColor = isSelected ? Colors.White : (Color)Application.Current!.Resources["CharcoalBrown"]
            };
            button.Clicked += (_, _) =>
            {
                _selectedFilter = category;
                BuildCategoryTabs();
                Reload();
            };

            CategoryTabs.Children.Add(button);
        }
    }

    private void Reload()
    {
        AnnouncementsList.Children.Clear();

        if (Session.CurrentUser is not { } user)
            return;

        var all = Database.Connection.Table<Announcement>()
            .Where(a => a.SchoolName == user.SchoolName)
            .ToList();

        var query = all
            .Where(a => _selectedFilter == null || a.Category == _selectedFilter)
            .Where(a => a.Category != AnnouncementCategory.Klasa || a.ClassName == user.ClassName)
            .OrderByDescending(a => a.Date)
            .ToList();

        foreach (var item in query)
        {
            var titleLabel = new Label { Text = item.Title, FontAttributes = FontAttributes.Bold };
            var bodyLabel = new Label { Text = item.Body, FontSize = 13, Opacity = 0.75 };
            var dateLabel = new Label { Text = item.Date.ToString("dd.MM.yyyy"), FontSize = 11, Opacity = 0.5 };

            AnnouncementsList.Children.Add(new Border
            {
                Padding = 16,
                Stroke = (Color)Application.Current!.Resources["CharcoalBrown"],
                BackgroundColor = (Color)Application.Current!.Resources["Porcelain"],
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
                Content = new VerticalStackLayout { Spacing = 4, Children = { titleLabel, bodyLabel, dateLabel } }
            });
        }

        if (query.Count == 0)
            AnnouncementsList.Children.Add(new Label { Text = "Brak komunikatów.", Opacity = 0.6, HorizontalOptions = LayoutOptions.Center });
    }
}
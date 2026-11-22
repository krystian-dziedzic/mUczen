using mUczen.Services;

namespace mUczen.Views;

public class MorePage : ContentPage
{
    private readonly VerticalStackLayout _menu = new() { Spacing = 10 };

    public MorePage()
    {
        Title = "Więcej";

        var grid = new Grid
        {
            Padding = new Thickness(20),
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star)
            }
        };
        grid.Add(new Label
        {
            Text = "Więcej",
            FontSize = 22,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 10, 0, 10)
        }, 0, 0);
        grid.Add(new ScrollView { Content = _menu }, 0, 1);

        Content = grid;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _menu.Children.Clear();

        if (Session.CurrentUser?.IsAdmin == true)
        {
            AddItem("Panel administratora",
                "Użytkownicy, przedmioty, oceny, plan lekcji i frekwencja",
                () => Navigation.PushAsync(new AdminPage()));
        }

        if (_menu.Children.Count == 0)
        {
            _menu.Children.Add(new Label
            {
                Text = "Brak dodatkowych opcji.",
                Opacity = 0.6,
                HorizontalOptions = LayoutOptions.Center
            });
        }
    }

    private void AddItem(string title, string subtitle, Func<Task> onTap)
    {
        var texts = new VerticalStackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center };
        texts.Children.Add(new Label { Text = title, FontAttributes = FontAttributes.Bold, FontSize = 16 });
        texts.Children.Add(new Label { Text = subtitle, FontSize = 12, Opacity = 0.65 });

        var row = new Grid
        {
            ColumnSpacing = 12,
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };
        row.Add(texts, 0);
        row.Add(new Label
        {
            Text = "›",
            FontSize = 20,
            TextColor = (Color)Application.Current!.Resources["Cerulean"],
            VerticalOptions = LayoutOptions.Center
        }, 1);

        var card = new Border
        {
            Padding = 16,
            Stroke = (Color)Application.Current!.Resources["CharcoalBrown"],
            BackgroundColor = (Color)Application.Current!.Resources["Porcelain"],
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
            Content = row
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, _) => await onTap();
        card.GestureRecognizers.Add(tap);

        _menu.Children.Add(card);
    }
}

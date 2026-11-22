using mUczen.Services;

namespace mUczen.Views;

public partial class MorePage : ContentPage
{
    public MorePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        MenuList.Children.Clear();

        if (Session.CurrentUser?.IsAdmin == true)
        {
            AddItem("Panel administratora",
                "Użytkownicy, przedmioty, oceny, plan lekcji i frekwencja",
                () => Navigation.PushAsync(new AdminPage()));
        }

        if (MenuList.Children.Count == 0)
        {
            MenuList.Children.Add(new Label
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
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
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

        MenuList.Children.Add(card);
    }

    private async void LogoutClicked(object sender, EventArgs e)
    {
        Session.CurrentUser = null;
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
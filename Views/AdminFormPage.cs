using mUczen.Services;

namespace mUczen.Views;

public abstract class AdminFormPage : ContentPage
{
    private readonly VerticalStackLayout _form = new() { Spacing = 6 };

    protected AdminFormPage(string title, string saveText)
    {
        Title = title;

        var cerulean = (Color)Application.Current!.Resources["Cerulean"];

        var header = new Label
        {
            Text = title,
            FontSize = 22,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 10, 0, 10)
        };

        var save = new Button
        {
            Text = saveText,
            TextColor = Colors.White,
            BackgroundColor = cerulean,
            CornerRadius = 12,
            HeightRequest = 52,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 10, 0, 0),
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(cerulean),
                Opacity = 0.35f,
                Radius = 14,
                Offset = new Point(0, 6)
            }
        };
        save.Clicked += async (_, _) =>
        {
            try
            {
                if (await SaveAsync())
                    await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Błąd", ex.Message, "OK");
            }
        };

        var grid = new Grid
        {
            Padding = new Thickness(20),
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto)
            }
        };
        grid.Add(header, 0, 0);
        grid.Add(new ScrollView { Content = _form }, 0, 1);
        grid.Add(save, 0, 2);

        Content = grid;
    }

    protected abstract Task<bool> SaveAsync();

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Session.CurrentUser?.IsAdmin != true)
            await Navigation.PopAsync();
    }

    protected Task Alert(string message) => DisplayAlertAsync("Błąd", message, "OK");

    private void AddCaption(string text)
    {
        _form.Children.Add(new Label
        {
            Text = text,
            FontSize = 13,
            Opacity = 0.85,
            Margin = new Thickness(4, 6, 0, 0)
        });
    }

    private static Border Field(View content) => new()
    {
        HeightRequest = 52,
        BackgroundColor = Colors.White,
        Stroke = (Color)Application.Current!.Resources["CharcoalBrown"],
        StrokeThickness = 1,
        StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
        Content = content
    };

    protected Picker AddPicker(string caption, string title)
    {
        AddCaption(caption);

        var picker = new Picker
        {
            Title = title,
            BackgroundColor = Colors.Transparent,
            TextColor = (Color)Application.Current!.Resources["CharcoalBrown"],
            TitleColor = Colors.Grey,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(12, 0, 0, 0)
        };

        _form.Children.Add(Field(picker));
        return picker;
    }

    protected Entry AddEntry(string caption, string? placeholder, Keyboard? keyboard = null, string? text = null)
    {
        AddCaption(caption);

        var entry = new Entry
        {
            Placeholder = placeholder,
            Text = text,
            Keyboard = keyboard ?? Keyboard.Default,
            BackgroundColor = Colors.Transparent,
            TextColor = (Color)Application.Current!.Resources["CharcoalBrown"],
            PlaceholderColor = Colors.Grey,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(12, 0, 0, 0)
        };

        _form.Children.Add(Field(entry));
        return entry;
    }

    protected Label AddHint()
    {
        var hint = new Label { FontSize = 12, Opacity = 0.65, Margin = new Thickness(4, 0, 0, 0) };
        _form.Children.Add(hint);
        return hint;
    }
}

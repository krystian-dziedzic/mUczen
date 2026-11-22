using mUczen.Models;
using mUczen.Services;
using System.Globalization;

namespace mUczen.Views;

public abstract class AdminListPage : ContentPage
{
    protected static readonly CultureInfo Pl = new("pl-PL");

    protected static readonly string[] DayLabels =
        { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };

    protected static readonly DayOfWeek[] DayValues =
    {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
        DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
    };

    private readonly VerticalStackLayout _list = new() { Spacing = 10 };

    protected AdminListPage(string title, string addText)
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

        var addButton = new Button
        {
            Text = addText,
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
        addButton.Clicked += async (_, _) => await Safe(AddAsync);

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
        grid.Add(new ScrollView { Content = _list }, 0, 1);
        grid.Add(addButton, 0, 2);

        Content = grid;
    }

    protected abstract void Build();
    protected abstract Task AddAsync();

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Session.CurrentUser?.IsAdmin != true)
        {
            await Navigation.PopAsync();
            return;
        }

        Refresh();
    }

    protected void Refresh()
    {
        _list.Children.Clear();
        Build();

        if (_list.Children.Count == 0)
        {
            _list.Children.Add(new Label
            {
                Text = "Brak danych.",
                Opacity = 0.6,
                HorizontalOptions = LayoutOptions.Center
            });
        }
    }

    protected void AddCard(string title, string? subtitle, string? right, Func<Task> onTap)
    {
        var texts = new VerticalStackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center };
        texts.Children.Add(new Label { Text = title, FontAttributes = FontAttributes.Bold });
        if (!string.IsNullOrWhiteSpace(subtitle))
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

        if (!string.IsNullOrWhiteSpace(right))
        {
            row.Add(new Label
            {
                Text = right,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            }, 1);
        }

        var card = new Border
        {
            Padding = 16,
            Stroke = (Color)Application.Current!.Resources["CharcoalBrown"],
            BackgroundColor = (Color)Application.Current!.Resources["Porcelain"],
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
            Content = row
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, _) => await Safe(onTap);
        card.GestureRecognizers.Add(tap);

        _list.Children.Add(card);
    }

    private async Task Safe(Func<Task> action)
    {
        try
        {
            await action();
            Refresh();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Błąd", ex.Message, "OK");
        }
    }

    protected Task<string?> Ask(string title, string? initial = null, string? placeholder = null, Keyboard? keyboard = null)
        => DisplayPromptAsync(title, string.Empty, "OK", "Anuluj", placeholder, -1, keyboard ?? Keyboard.Default, initial ?? string.Empty);

    protected Task Alert(string message) => DisplayAlertAsync("Błąd", message, "OK");

    protected Task<bool> Confirm(string message) => DisplayAlertAsync("Potwierdź", message, "Usuń", "Anuluj");

    protected static string FullName(User? u) => u == null ? "(usunięty użytkownik)" : $"{u.FirstName} {u.LastName}";

    protected static string DayLabel(DayOfWeek d) => DayLabels[Array.IndexOf(DayValues, d)];

    protected static bool TryParseGrade(string? text, out double value) => AdminUtil.TryParseGrade(text, out value);

    protected static bool TryParseDate(string? text, out DateTime date) => AdminUtil.TryParseDate(text, out date);

    protected async Task<User?> PickUser()
    {
        var users = Database.Connection.Table<User>().ToList()
            .OrderBy(u => u.ClassName).ThenBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();

        if (users.Count == 0)
        {
            await Alert("Brak użytkowników.");
            return null;
        }

        var labels = AdminUtil.UserLabels(users).ToArray();
        var choice = await DisplayActionSheetAsync("Wybierz ucznia", "Anuluj", null, labels);
        var index = Array.IndexOf(labels, choice);
        return index >= 0 ? users[index] : null;
    }

    protected async Task<Lesson?> PickLesson()
    {
        var lessons = Database.Connection.Table<Lesson>().ToList()
            .OrderBy(l => Array.IndexOf(DayValues, l.DayOfWeek)).ThenBy(l => l.Subject).ToList();

        if (lessons.Count == 0)
        {
            await Alert("Brak lekcji. Najpierw dodaj lekcję w planie.");
            return null;
        }

        var labels = lessons.Select(l => $"{l.Subject} · {DayLabel(l.DayOfWeek)} (#{l.Id})").ToArray();
        var choice = await DisplayActionSheetAsync("Wybierz lekcję", "Anuluj", null, labels);
        var index = Array.IndexOf(labels, choice);
        return index >= 0 ? lessons[index] : null;
    }
}

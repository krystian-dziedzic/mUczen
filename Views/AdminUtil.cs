using mUczen.Models;
using System.Globalization;

namespace mUczen.Views;

internal static class AdminUtil
{
    public static readonly CultureInfo Pl = new("pl-PL");

    public static string UserLabel(User u)
    {
        var cls = string.IsNullOrWhiteSpace(u.ClassName) ? "bez klasy" : u.ClassName;
        return $"{u.FirstName} {u.LastName} ({cls})";
    }

    public static List<string> UserLabels(IList<User> users)
    {
        var labels = users.Select(UserLabel).ToList();
        return labels
            .Select((l, i) => labels.Count(x => x == l) > 1 ? $"{l} #{users[i].Id}" : l)
            .ToList();
    }

    public static bool TryParseGrade(string? text, out double value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(text)) return false;
        return double.TryParse(text.Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
               && value >= 1 && value <= 6;
    }

    public static bool TryParseDate(string? text, out DateTime date)
        => DateTime.TryParseExact(text?.Trim(), new[] { "dd.MM.yyyy", "d.M.yyyy", "yyyy-MM-dd" },
            Pl, DateTimeStyles.None, out date);
}

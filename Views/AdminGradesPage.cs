using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public class AdminGradesPage : AdminListPage
{
    public AdminGradesPage() : base("Oceny", "Dodaj ocenę") { }

    protected override void Build()
    {
        var users = Database.Connection.Table<User>().ToList().ToDictionary(u => u.Id);
        var grades = Database.Connection.Table<Grade>().ToList().OrderByDescending(g => g.Date);

        foreach (var grade in grades)
        {
            var g = grade;
            users.TryGetValue(g.UserId, out var owner);
            var who = owner == null ? FullName(null) : AdminUtil.UserLabel(owner);

            AddCard(g.Subject ?? "-",
                $"{who} · {g.Date.ToString("dd.MM.yyyy", Pl)}",
                g.Value.ToString("0.##", Pl),
                () => GradeActions(g));
        }
    }

    protected override Task AddAsync() => Navigation.PushAsync(new AdminGradeFormPage());

    private async Task GradeActions(Grade g)
    {
        var choice = await DisplayActionSheetAsync(g.Subject ?? "Ocena", "Anuluj", "Usuń", "Edytuj");

        if (choice == "Edytuj")
        {
            await Navigation.PushAsync(new AdminGradeFormPage(g));
        }
        else if (choice == "Usuń")
        {
            if (!await Confirm("Usunąć tę ocenę?")) return;
            Database.Connection.Delete(g);
        }
    }
}

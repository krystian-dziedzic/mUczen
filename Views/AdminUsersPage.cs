using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public class AdminUsersPage : AdminListPage
{
    public AdminUsersPage() : base("Użytkownicy", "Dodaj użytkownika") { }

    protected override void Build()
    {
        var users = Database.Connection.Table<User>().ToList()
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName);

        foreach (var user in users)
        {
            var u = user;
            var subtitle = string.Join(" · ",
                new[] { u.StudentIdCard, u.ClassName, u.SchoolName }.Where(s => !string.IsNullOrWhiteSpace(s)));

            AddCard(FullName(u), subtitle, u.IsAdmin ? "Admin" : null, () => UserActions(u));
        }
    }

    protected override async Task AddAsync()
    {
        var first = await Ask("Imię");
        if (string.IsNullOrWhiteSpace(first)) return;

        var last = await Ask("Nazwisko");
        if (string.IsNullOrWhiteSpace(last)) return;

        var card = await Ask("Numer legitymacji", keyboard: Keyboard.Numeric);
        if (string.IsNullOrWhiteSpace(card)) return;

        if (CardExists(card, null))
        {
            await Alert("Konto z tym numerem legitymacji już istnieje.");
            return;
        }

        var password = await Ask("Hasło (min. 8 znaków)");
        if (string.IsNullOrWhiteSpace(password)) return;

        if (password.Length < 8)
        {
            await Alert("Hasło powinno mieć co najmniej 8 znaków.");
            return;
        }

        var school = await Ask("Szkoła");
        if (school is null) return;

        var className = await Ask("Klasa");
        if (className is null) return;

        var user = new User
        {
            FirstName = first.Trim(),
            LastName = last.Trim(),
            StudentIdCard = card.Trim(),
            SchoolName = school.Trim(),
            ClassName = className.Trim()
        };
        user.Password = Database.hasher.HashPassword(user, password);

        Database.Connection.Insert(user);
    }

    private async Task UserActions(User u)
    {
        var adminAction = u.IsAdmin ? "Odbierz uprawnienia admina" : "Nadaj uprawnienia admina";

        var choice = await DisplayActionSheetAsync(FullName(u), "Anuluj", "Usuń",
            "Edytuj dane", "Zmień hasło", adminAction);

        bool isSelf = Session.CurrentUser?.Id == u.Id;

        if (choice == "Edytuj dane")
        {
            await EditUser(u);
        }
        else if (choice == "Zmień hasło")
        {
            var password = await Ask("Nowe hasło (min. 8 znaków)");
            if (string.IsNullOrWhiteSpace(password)) return;

            if (password.Length < 8)
            {
                await Alert("Hasło powinno mieć co najmniej 8 znaków.");
                return;
            }

            u.Password = Database.hasher.HashPassword(u, password);
            Database.Connection.Update(u);
        }
        else if (choice == adminAction)
        {
            if (isSelf)
            {
                await Alert("Nie możesz zmienić własnych uprawnień.");
                return;
            }

            u.IsAdmin = !u.IsAdmin;
            Database.Connection.Update(u);
        }
        else if (choice == "Usuń")
        {
            if (isSelf)
            {
                await Alert("Nie możesz usunąć własnego konta.");
                return;
            }

            if (!await Confirm($"Usunąć użytkownika {FullName(u)} wraz z jego ocenami i frekwencją?")) return;

            Database.Connection.Table<Grade>().Delete(g => g.UserId == u.Id);
            Database.Connection.Table<Attendance>().Delete(a => a.UserId == u.Id);
            Database.Connection.Delete(u);
        }
    }

    private async Task EditUser(User u)
    {
        var first = await Ask("Imię", u.FirstName);
        if (string.IsNullOrWhiteSpace(first)) return;

        var last = await Ask("Nazwisko", u.LastName);
        if (string.IsNullOrWhiteSpace(last)) return;

        var card = await Ask("Numer legitymacji", u.StudentIdCard, keyboard: Keyboard.Numeric);
        if (string.IsNullOrWhiteSpace(card)) return;

        if (CardExists(card, u.Id))
        {
            await Alert("Konto z tym numerem legitymacji już istnieje.");
            return;
        }

        var school = await Ask("Szkoła", u.SchoolName);
        if (school is null) return;

        var className = await Ask("Klasa", u.ClassName);
        if (className is null) return;

        u.FirstName = first.Trim();
        u.LastName = last.Trim();
        u.StudentIdCard = card.Trim();
        u.SchoolName = school.Trim();
        u.ClassName = className.Trim();

        Database.Connection.Update(u);

        if (Session.CurrentUser?.Id == u.Id)
            Session.CurrentUser = u;
    }

    private static bool CardExists(string card, int? exceptUserId)
    {
        card = card.Trim();
        return Database.Connection.Table<User>()
            .Any(x => x.StudentIdCard == card && (exceptUserId == null || x.Id != exceptUserId));
    }
}

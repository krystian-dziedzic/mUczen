using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public class AdminAttendancePage : AdminListPage
{
    public AdminAttendancePage() : base("Frekwencja", "Dodaj wpis") { }

    protected override void Build()
    {
        var users = Database.Connection.Table<User>().ToList().ToDictionary(u => u.Id);
        var lessons = Database.Connection.Table<Lesson>().ToList().ToDictionary(l => l.Id);
        var entries = Database.Connection.Table<Attendance>().ToList().OrderByDescending(a => a.Date);

        foreach (var entry in entries)
        {
            var a = entry;
            users.TryGetValue(a.UserId, out var owner);
            var lessonName = lessons.TryGetValue(a.LessonId, out var lesson) ? lesson.Subject : "(usunięta lekcja)";

            AddCard(FullName(owner),
                $"{lessonName} · {a.Date.ToString("dd.MM.yyyy", Pl)}",
                a.IsPresent ? "Obecny" : "Nieobecny",
                () => EntryActions(a));
        }
    }

    protected override async Task AddAsync()
    {
        var user = await PickUser();
        if (user == null) return;

        var lesson = await PickLesson();
        if (lesson == null) return;

        var dateText = await Ask("Data (dd.mm.rrrr)", DateTime.Today.ToString("dd.MM.yyyy", Pl));
        if (dateText is null) return;

        if (!TryParseDate(dateText, out var date))
        {
            await Alert("Nieprawidłowa data. Użyj formatu dd.mm.rrrr.");
            return;
        }

        var status = await DisplayActionSheetAsync("Status", "Anuluj", null, "Obecny", "Nieobecny");
        if (status != "Obecny" && status != "Nieobecny") return;

        Database.Connection.Insert(new Attendance
        {
            UserId = user.Id,
            LessonId = lesson.Id,
            Date = date,
            IsPresent = status == "Obecny"
        });
    }

    private async Task EntryActions(Attendance a)
    {
        var choice = await DisplayActionSheetAsync("Wpis frekwencji", "Anuluj", "Usuń", "Zmień status", "Zmień datę");

        if (choice == "Zmień status")
        {
            a.IsPresent = !a.IsPresent;
            Database.Connection.Update(a);
        }
        else if (choice == "Zmień datę")
        {
            var dateText = await Ask("Data (dd.mm.rrrr)", a.Date.ToString("dd.MM.yyyy", Pl));
            if (dateText is null) return;

            if (!TryParseDate(dateText, out var date))
            {
                await Alert("Nieprawidłowa data. Użyj formatu dd.mm.rrrr.");
                return;
            }

            a.Date = date;
            Database.Connection.Update(a);
        }
        else if (choice == "Usuń")
        {
            if (!await Confirm("Usunąć ten wpis frekwencji?")) return;
            Database.Connection.Delete(a);
        }
    }
}

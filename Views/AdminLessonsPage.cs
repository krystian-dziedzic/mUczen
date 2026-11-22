using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public class AdminLessonsPage : AdminListPage
{
    public AdminLessonsPage() : base("Plan lekcji", "Dodaj lekcję") { }

    protected override void Build()
    {
        var lessons = Database.Connection.Table<Lesson>().ToList()
            .OrderBy(l => Array.IndexOf(DayValues, l.DayOfWeek))
            .ThenBy(l => l.Subject);

        foreach (var lesson in lessons)
        {
            var l = lesson;
            var teacher = string.IsNullOrWhiteSpace(l.Teacher) ? "brak nauczyciela" : l.Teacher;

            AddCard(l.Subject ?? "-",
                $"{teacher} · {(int)l.Duration.TotalMinutes} min",
                DayLabel(l.DayOfWeek),
                () => LessonActions(l));
        }
    }

    protected override async Task AddAsync()
    {
        var lesson = new Lesson();
        if (await Fill(lesson))
            Database.Connection.Insert(lesson);
    }

    private async Task LessonActions(Lesson l)
    {
        var choice = await DisplayActionSheetAsync(l.Subject ?? "Lekcja", "Anuluj", "Usuń", "Edytuj");

        if (choice == "Edytuj")
        {
            if (await Fill(l))
                Database.Connection.Update(l);
        }
        else if (choice == "Usuń")
        {
            if (!await Confirm("Usunąć tę lekcję wraz z powiązanymi wpisami frekwencji?")) return;

            Database.Connection.Table<Attendance>().Delete(a => a.LessonId == l.Id);
            Database.Connection.Delete(l);
        }
    }

    private async Task<bool> Fill(Lesson l)
    {
        bool isNew = l.Id == 0;

        var subject = await Ask("Przedmiot", l.Subject);
        if (string.IsNullOrWhiteSpace(subject)) return false;

        var dayChoice = await DisplayActionSheetAsync("Dzień tygodnia", "Anuluj", null, DayLabels);
        int dayIndex = Array.IndexOf(DayLabels, dayChoice);
        if (dayIndex < 0) return false;

        var minutesText = await Ask("Czas trwania (minuty)",
            isNew ? "45" : ((int)l.Duration.TotalMinutes).ToString(), keyboard: Keyboard.Numeric);
        if (minutesText is null) return false;

        if (!int.TryParse(minutesText.Trim(), out var minutes) || minutes <= 0 || minutes > 600)
        {
            await Alert("Czas trwania musi być liczbą minut od 1 do 600.");
            return false;
        }

        var teacher = await Ask("Nauczyciel", l.Teacher);
        if (teacher is null) return false;

        l.Subject = subject.Trim();
        l.DayOfWeek = DayValues[dayIndex];
        l.Duration = TimeSpan.FromMinutes(minutes);
        l.Teacher = teacher.Trim();
        return true;
    }
}

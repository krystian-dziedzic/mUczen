using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public class AdminSubjectsPage : AdminListPage
{
    public AdminSubjectsPage() : base("Przedmioty", "Dodaj przedmiot") { }

    protected override void Build()
    {
        var subjects = Database.Connection.Table<ClassSubject>().ToList()
            .OrderBy(s => s.ClassName).ThenBy(s => s.Name);

        foreach (var subject in subjects)
        {
            var s = subject;
            AddCard(s.Name ?? "-", null, s.ClassName, () => SubjectActions(s));
        }
    }

    protected override Task AddAsync() => Navigation.PushAsync(new AdminSubjectFormPage());

    private async Task SubjectActions(ClassSubject s)
    {
        var choice = await DisplayActionSheetAsync($"{s.Name} ({s.ClassName})", "Anuluj", "Usuń", "Edytuj");

        if (choice == "Edytuj")
        {
            await Navigation.PushAsync(new AdminSubjectFormPage(s));
        }
        else if (choice == "Usuń")
        {
            if (!await Confirm($"Usunąć przedmiot {s.Name} z klasy {s.ClassName}? Wystawione już oceny zostaną zachowane.")) return;
            Database.Connection.Delete(s);
        }
    }
}

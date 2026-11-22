using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public class AdminSubjectFormPage : AdminFormPage
{
    private readonly ClassSubject? _subject;
    private readonly List<string> _classes;
    private readonly Entry _nameEntry;
    private readonly Picker _classPicker;

    public AdminSubjectFormPage(ClassSubject? subject = null)
        : base(subject == null ? "Dodaj przedmiot" : "Edytuj przedmiot", "Zapisz")
    {
        _subject = subject;

        _classes = Database.Connection.Table<User>().ToList().Select(u => u.ClassName)
            .Concat(Database.Connection.Table<ClassSubject>().ToList().Select(s => s.ClassName))
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Create(AdminUtil.Pl, true))
            .ToList();

        _nameEntry = AddEntry("Nazwa przedmiotu", "np. Matematyka", null, subject?.Name);

        _classPicker = AddPicker("Klasa", "Wybierz klasę");
        _classPicker.ItemsSource = _classes;

        var hint = AddHint();
        if (_classes.Count == 0)
            hint.Text = "Brak klas. Ustaw klasę uczniom w Użytkownicy → Edytuj dane.";

        if (subject != null)
        {
            _classPicker.SelectedIndex = _classes.FindIndex(c =>
                string.Equals(c, subject.ClassName, StringComparison.OrdinalIgnoreCase));
        }
    }

    protected override async Task<bool> SaveAsync()
    {
        var name = _nameEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            await Alert("Podaj nazwę przedmiotu.");
            return false;
        }

        if (_classPicker.SelectedIndex < 0)
        {
            await Alert("Wybierz klasę.");
            return false;
        }

        var className = _classes[_classPicker.SelectedIndex];

        bool duplicate = Database.Connection.Table<ClassSubject>().ToList().Any(s =>
            s.Id != (_subject?.Id ?? 0) &&
            string.Equals(s.ClassName, className, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            await Alert("Ta klasa ma już taki przedmiot.");
            return false;
        }

        if (_subject == null)
        {
            Database.Connection.Insert(new ClassSubject { Name = name, ClassName = className });
            return true;
        }

        var oldName = _subject.Name;
        var oldClass = _subject.ClassName;

        _subject.Name = name;
        _subject.ClassName = className;
        Database.Connection.Update(_subject);

        if (!string.Equals(oldName, name, StringComparison.Ordinal) &&
            string.Equals(oldClass, className, StringComparison.OrdinalIgnoreCase))
        {
            var ids = Database.Connection.Table<User>().ToList()
                .Where(u => string.Equals(u.ClassName, className, StringComparison.OrdinalIgnoreCase))
                .Select(u => u.Id).ToHashSet();

            var affected = Database.Connection.Table<Grade>().ToList()
                .Where(g => ids.Contains(g.UserId) && g.Subject == oldName).ToList();

            foreach (var g in affected) g.Subject = name;
            Database.Connection.UpdateAll(affected);
        }

        return true;
    }
}

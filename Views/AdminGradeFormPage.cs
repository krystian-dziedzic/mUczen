using mUczen.Models;
using mUczen.Services;

namespace mUczen.Views;

public class AdminGradeFormPage : AdminFormPage
{
    private readonly Grade? _grade;
    private readonly List<User> _users;
    private List<string> _subjects = new();

    private readonly Picker _userPicker;
    private readonly Picker _subjectPicker;
    private readonly Label _hint;
    private readonly Entry _valueEntry;
    private readonly Entry _dateEntry;

    public AdminGradeFormPage(Grade? grade = null)
        : base(grade == null ? "Dodaj ocenę" : "Edytuj ocenę", "Zapisz")
    {
        _grade = grade;
        _users = Database.Connection.Table<User>().ToList()
            .OrderBy(u => u.ClassName).ThenBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();

        _userPicker = AddPicker("Uczeń", "Wybierz ucznia");
        _userPicker.ItemsSource = AdminUtil.UserLabels(_users);

        _subjectPicker = AddPicker("Przedmiot", "Najpierw wybierz ucznia");
        _hint = AddHint();

        _valueEntry = AddEntry("Ocena (1-6)", "np. 4,5", Keyboard.Numeric,
            grade?.Value.ToString("0.##", AdminUtil.Pl));
        _dateEntry = AddEntry("Data", "dd.mm.rrrr", null,
            (grade?.Date ?? DateTime.Today).ToString("dd.MM.yyyy", AdminUtil.Pl));

        _userPicker.SelectedIndexChanged += (_, _) => LoadSubjects(null);

        if (grade != null)
        {
            _userPicker.SelectedIndex = _users.FindIndex(u => u.Id == grade.UserId);
            LoadSubjects(grade.Subject);
        }
    }

    private void LoadSubjects(string? preselect)
    {
        int index = _userPicker.SelectedIndex;

        if (index < 0)
        {
            _subjects = new List<string>();
        }
        else
        {
            var className = _users[index].ClassName ?? string.Empty;

            _subjects = Database.Connection.Table<ClassSubject>().ToList()
                .Where(s => string.Equals(s.ClassName ?? string.Empty, className, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.Name ?? string.Empty)
                .OrderBy(n => n, StringComparer.Create(AdminUtil.Pl, true))
                .ToList();
        }

        if (preselect != null && !_subjects.Contains(preselect))
            _subjects.Add(preselect);

        _subjectPicker.Title = index < 0 ? "Najpierw wybierz ucznia" : "Wybierz przedmiot";
        _subjectPicker.ItemsSource = _subjects.ToList();
        _subjectPicker.SelectedIndex = preselect != null ? _subjects.IndexOf(preselect) : -1;

        _hint.Text = index >= 0 && _subjects.Count == 0
            ? "Ta klasa nie ma jeszcze przedmiotów. Dodaj je w Panelu administratora → Przedmioty."
            : string.Empty;
    }

    protected override async Task<bool> SaveAsync()
    {
        if (_userPicker.SelectedIndex < 0)
        {
            await Alert("Wybierz ucznia.");
            return false;
        }

        if (_subjectPicker.SelectedIndex < 0)
        {
            await Alert("Wybierz przedmiot.");
            return false;
        }

        if (!AdminUtil.TryParseGrade(_valueEntry.Text, out var value))
        {
            await Alert("Ocena musi być liczbą od 1 do 6.");
            return false;
        }

        if (!AdminUtil.TryParseDate(_dateEntry.Text, out var date))
        {
            await Alert("Nieprawidłowa data. Użyj formatu dd.mm.rrrr.");
            return false;
        }

        var grade = _grade ?? new Grade();
        grade.UserId = _users[_userPicker.SelectedIndex].Id;
        grade.Subject = _subjects[_subjectPicker.SelectedIndex];
        grade.Value = value;
        grade.Date = date;

        if (_grade == null)
            Database.Connection.Insert(grade);
        else
            Database.Connection.Update(grade);

        return true;
    }
}

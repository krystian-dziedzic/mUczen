using SQLite;

namespace mUczen.Models
{
    public class ClassSubject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string? ClassName { get; set; }

        public string? Name { get; set; }
    }
}

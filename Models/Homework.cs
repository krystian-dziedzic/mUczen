using SQLite;

namespace mUczen.Models
{
    public class Homework
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string? SchoolName { get; set; }
        [Indexed]
        public string? ClassName { get; set; }

        public string? Subject { get; set; }
        public string? Title { get; set; }
        public DateTime DueDate { get; set; }
    }
}
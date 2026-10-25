using SQLite;

namespace mUczen.Models
{
    public class Lesson
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Subject { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Duration { get; set; }
        public string? Teacher { get; set; }
    }
}
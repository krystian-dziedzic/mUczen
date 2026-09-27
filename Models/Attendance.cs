using SQLite;

namespace mUczen.Models
{
    public class Attendance
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; }

        public int LessonId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}
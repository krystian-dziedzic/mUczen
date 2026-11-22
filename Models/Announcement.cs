using SQLite;

namespace mUczen.Models
{
    public enum AnnouncementCategory { Szkola, Klasa, Nauczyciele }

    public class Announcement
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string? SchoolName { get; set; }
        public string? ClassName { get; set; }

        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime Date { get; set; }
        public AnnouncementCategory Category { get; set; }
    }
}
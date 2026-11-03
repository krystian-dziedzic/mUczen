using SQLite;

namespace mUczen.Models
{
    public class Grade
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; }

        public string? Subject { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}
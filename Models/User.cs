using SQLite;
namespace mUczen.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string? StudentIdCard { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
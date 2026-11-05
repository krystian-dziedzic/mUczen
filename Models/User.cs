using SQLite;

namespace mUczen.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed(Unique = true)]
        public string? Username { get; set; }

        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
using SQLite;
using Microsoft.AspNetCore.Identity;
namespace mUczen.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? StudentIdCard { get; set; }
        public PasswordHasher<User>? Password { get; set; }
        public string? FirstName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
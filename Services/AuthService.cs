using mUczen.Models;

namespace mUczen.Services
{
    public enum RegisterResult { Success, EmptyFields, PasswordsDoNotMatch, PasswordTooShort, UsernameTaken }

    public static class AuthService
    {
        public static User? CurrentUser { get; private set; }

        public static RegisterResult Register(string? username, string? password, string? repeatPassword)
        {
            username = username?.Trim();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return RegisterResult.EmptyFields;
            if (password != repeatPassword)
                return RegisterResult.PasswordsDoNotMatch;
            if (password.Length < 6)
                return RegisterResult.PasswordTooShort;
            if (Database.Connection.Table<User>().Any(u => u.Username == username))
                return RegisterResult.UsernameTaken;

            var (hash, salt) = PasswordHasher.Hash(password);
            Database.Connection.Insert(new User
            {
                Username = username,
                PasswordHash = hash,
                PasswordSalt = salt,
                IsAdmin = false
            });
            return RegisterResult.Success;
        }

        public static bool Login(string? username, string? password)
        {
            username = username?.Trim();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            var user = Database.Connection.Table<User>().FirstOrDefault(u => u.Username == username);
            if (user?.PasswordHash == null || user.PasswordSalt == null)
                return false;
            if (!PasswordHasher.Verify(password, user.PasswordHash, user.PasswordSalt))
                return false;

            CurrentUser = user;
            return true;
        }

        public static void Logout() => CurrentUser = null;
    }
}
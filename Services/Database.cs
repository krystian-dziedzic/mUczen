using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using mUczen.Models;

namespace mUczen.Services
{
    public class Database
    {
        private static SQLiteConnection Connection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mUczen.db"));
        public static void Initialize()
        {
            Connection.CreateTable<Models.User>();
        }
        public void insertUser(string username, string password, string email)
        {
            Connection.Insert(new User
            {
                FirstName = username,
                Email = email,
                Password = password
            });
        }
    }
}

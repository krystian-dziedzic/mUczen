using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using mUczen.Models;
using mUczen.Resources;

namespace mUczen.Services
{
    public class Database
    {
        private static SQLiteConnection Connection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Resources.Constants.DatabaseFilename));
        public static void Initialize()
        {
            Connection.CreateTable<Models.User>();
            Connection.CreateTable<Models.Grades>();
            Connection.CreateTable<Models.Schedule>();
            Connection.CreateTable<Models.Attendance>();
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

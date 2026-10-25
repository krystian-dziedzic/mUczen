using Microsoft.AspNetCore.Identity;
using mUczen.Models;
using mUczen.Resources;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace mUczen.Services
{
    public class Database
    {
        public static SQLiteConnection Connection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Resources.Constants.DatabaseFilename));
        public static void Initialize()
        {
            Connection.CreateTable<Grade>();
            Connection.CreateTable<Lesson>();
            Connection.CreateTable<Attendance>();
            Connection.CreateTable<User>();
        }
        public static PasswordHasher<User> hasher = new PasswordHasher<User>();
    }
}

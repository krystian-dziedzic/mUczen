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
        internal static SQLiteConnection Connection = new SQLiteConnection(Constants.DatabasePath);
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

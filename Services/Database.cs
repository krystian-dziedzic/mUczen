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
        private static readonly SQLiteConnection Connection = new SQLiteConnection(Constants.DatabasePath);
        public static void Initialize()
        {
            Connection.CreateTable<Grade>();
            Connection.CreateTable<Lesson>();
            Connection.CreateTable<Attendance>();
            Connection.CreateTable<User>();
        }
    }
}

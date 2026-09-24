using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace mUczen.Services
{
    public class Database
    {
        public static SQLiteConnection Connection = new SQLiteConnection("mUczen.db");
        public static void Initialize()
        {
            Connection.CreateTable<Models.User>();
        }
        public void insertUser(string username, string password, string email)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace mUczen.Models
{
    public class Schedule
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Week { get; set; }
        public List<Lesson>? Lessons { get; set; }
    }
    public class Lesson
    {
        public string? Subject { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Teacher { get; set; }
    }
}

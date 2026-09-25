using System;
using System.Collections.Generic;
using System.Text;

namespace mUczen.Models
{
    internal class Attendance
    {
        List<Tuple<DateTime, bool>>? AttendanceList;
        //DateTime - Data lekcji, bool - Obecność (true) lub nieobecność (false)
    }
}

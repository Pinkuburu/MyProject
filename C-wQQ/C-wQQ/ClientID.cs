using System;
using System.Collections.Generic;
using System.Text;

namespace C_wQQ
{
    class ClientID
    {
        public static string GenerateClientID()
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(0, 99) + "" + GetTime(DateTime.Now) / 1000000;
        }
        public static long GetTime(DateTime dateTime)
        {
            DateTime startDate = new DateTime(1970, 1, 1);
            DateTime endDate = dateTime.ToUniversalTime();
            TimeSpan span = endDate - startDate;
            return (long)(span.TotalMilliseconds + 0.5);
        }
    }
}

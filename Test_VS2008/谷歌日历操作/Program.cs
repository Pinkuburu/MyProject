using System;
using System.Collections.Generic;
using System.Text;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Calendar;

namespace 谷歌日历操作
{
    class Program
    {
        static void Main(string[] args)
        {
            string calendarURI = "http://www.google.com/calendar/feeds/default/allcalendars/full";
            string userName = "cupid0426@gmail.com";
            string passWord = "677521";

            //创建日历服务对象
            CalendarService services = new CalendarService("CalendarTestApp");
            services.setUserCredentials(userName, passWord);

            CalendarQuery query = new CalendarQuery(); 
            query.Uri = new Uri("http://www.google.com/calendar/feeds/default/allcalendars/full");
            CalendarFeed resultFeed = (CalendarFeed)services.Query(query); 
            Console.WriteLine("Your calendars:\n"); 
            foreach (CalendarEntry entry in resultFeed.Entries) 
            { 
                Console.WriteLine(entry.Title.Text + "\n"); 
            }
            Console.ReadKey();
        }
    }
}

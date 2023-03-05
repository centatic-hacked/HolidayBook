using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HolidayBook.Overview
{
    public class DateCalculator
    {
        public int MyProperty { get; set; }
        public static DateTime calcStartDate()
        {
            string curDate = DateTime.Now.ToString("dd-MM-yyyy");
            curDate = curDate.Substring(2);
            curDate = 01 + curDate;
            return DateTime.Parse(curDate);
        }

        public static DateTime calcEndDate()
        {
            DateTime today = DateTime.Now;
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            return endOfMonth;
        }

        public static DateTime calcFirstDayNextMonth()
        {
            DateTime today = DateTime.Today;
            DateTime nextMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month)).AddDays(1);
            return nextMonth;
        }

        public static DateTime newDisplayDate(DateTime dat)
        {
            string curDate = dat.ToString("dd-MM-yyyy");
            curDate = curDate.Substring(2);
            curDate = 01 + curDate;
            return DateTime.Parse(curDate);
        }

        public static DateTime calcEndDate2(DateTime dt)
        {
            DateTime endOfMonth = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
            return endOfMonth;
        }
    }
}


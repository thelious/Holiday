﻿using System;

namespace PublicHoliday
{
    /// <summary>
    /// Work out the date for Easter Sunday for specified year
    /// </summary>
    static class HolidayCalculator
    {
        /// <summary>
        /// Work out the date for Easter Sunday for specified year
        /// </summary>
        /// <param name="year">The year as an integer</param>
        /// <returns>Returns a datetime of Easter Sunday.</returns>
        public static DateTime GetEaster(int year)
        {
            //should be
            //Easter Monday  28 Mar 2005  17 Apr 2006  9 Apr 2007  24 Mar 2008

            //Oudin's Algorithm - http://www.smart.net/~mmontes/oudin.html
            var g = year % 19;
            var c = year / 100;
            var h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            var i = h - (h / 28) * (1 - (h / 28) * (29 / (h + 1)) * ((21 - g) / 11));
            var j = (year + year / 4 + i + 2 - c + c / 4) % 7;
            var p = i - j;
            var easterDay = 1 + (p + 27 + (p + 6) / 40) % 31;
            var easterMonth = 3 + (p + 26) / 30;

            return new DateTime(year, easterMonth, easterDay);
        }

        public static DateTime FixWeekend(DateTime hol)
        {
            if (hol.DayOfWeek == DayOfWeek.Sunday)
                hol = hol.AddDays(1);
            else if (hol.DayOfWeek == DayOfWeek.Saturday)
                hol = hol.AddDays(2);
            return hol;
        }

        public static DateTime FindFirstMonday(DateTime hol)
        {
            while (hol.DayOfWeek != DayOfWeek.Monday)
            {
                hol = hol.AddDays(1);
            }
            return hol;
        }

        public static DateTime FindPrevious(DateTime hol, DayOfWeek day)
        {
            while (hol.DayOfWeek != day)
                hol = hol.AddDays(-1);

            return hol;
        }

        /// <summary>
        /// Returns the next working day (Mon-Fri, not public holiday)
        /// after the specified date (or the same date)
        /// </summary>
        /// <param name="holidayCalendar">The holiday calendar.</param>
        /// <param name="dt">The date you wish to check</param>
        /// <returns>
        /// A date that is a working day
        /// </returns>
        public static DateTime NextWorkingDay(IPublicHolidays holidayCalendar, DateTime dt)
        {
            bool isWorkingDay = false;

            //loops through
            while (isWorkingDay == false)
            {
                //Mon-Fri and not bank holiday, it's okay
                if (dt.DayOfWeek != DayOfWeek.Saturday &&
                    dt.DayOfWeek != DayOfWeek.Sunday &&
                    !holidayCalendar.IsPublicHoliday(dt))
                    isWorkingDay = true;
                //it's Saturday, so skip to Monday
                else if (dt.DayOfWeek == DayOfWeek.Saturday)
                    dt = dt.AddDays(2);
                //it's Sunday, so skip to Monday
                else if (dt.DayOfWeek == DayOfWeek.Sunday)
                    dt = dt.AddDays(1);
                //it's Friday (bank holiday), so skip to Monday
                else if (dt.DayOfWeek == DayOfWeek.Friday)
                    dt = dt.AddDays(3);
                //it's Mon-Thu (bank holiday), so next day
                else
                    dt = dt.AddDays(1);
                //any of the addDays should now loop and retest
                //only Good Friday and Christmas Day should loop twice
                //(2 adjacent bank holidays)
            }
            return dt;
        }
    }
}

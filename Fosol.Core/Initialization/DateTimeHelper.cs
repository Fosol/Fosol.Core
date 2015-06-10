using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Initialization
{
    /// <summary>
    /// DateTimeHelper static class, provides methods to help with DateTime objects.
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Returns a DateTime representing the specified day in January for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime January(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 1, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in February for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime February(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 2, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in March for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime March(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 3, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in April for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime April(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 4, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in May for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime May(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 5, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in June for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime June(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 6, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in July for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime July(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 7, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in August for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime August(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 8, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in September for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime September(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 9, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in October for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime October(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 10, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in November for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime November(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 11, day);
        }

        /// <summary>
        /// Returns a DateTime representing the specified day in December for the specified year.
        /// </summary>
        /// <param name="day">Day of the month.  Default will result in today's day of the month.</param>
        /// <param name="year">The year.  Default will result in today's year.</param>
        /// <returns>New instance of a DateTime for the specified date.</returns>
        public static DateTime December(int day = 0, int year = 0)
        {
            if (day == 0)
                day = Optimization.FastDateTime.UtcNow.Day;
            if (year == 0)
                day = Optimization.FastDateTime.UtcNow.Year;
            return new DateTime(year, 12, day);
        }
    }
}

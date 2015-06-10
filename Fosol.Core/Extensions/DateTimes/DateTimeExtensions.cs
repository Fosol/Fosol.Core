using System;

namespace Fosol.Core.Extensions.DateTimes
{
    /// <summary>
    /// DateTimeExtensions static class, provides extension methods for DateTime objects.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calculates the age in years of a person based on their birthdate.
        /// </summary>
        /// <param name="birthdate">Birthdate of the person.</param>
        /// <returns>Age in years of the person.</returns>
        public static int GetAge(this DateTime birthdate)
        {
            var today = Optimization.FastDateTime.UtcNow;
            var bday = birthdate.ToUniversalTime();
            if (today.Month < bday.Month
                || (today.Month == bday.Month
                    && today.Day < bday.Day))
                return today.Year - bday.Year - 1;
            else
                return today.Year - bday.Year;
        }
    }
}

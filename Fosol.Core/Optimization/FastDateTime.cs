using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Optimization
{
    /// <summary>
    /// FastDateTime sealed class, provides an optimized way to retrieve the current DateTime value.
    /// </summary>
    public sealed class FastDateTime
    {
        #region Variables
        private static int _LastTicks = -1;
        private static DateTime _LastDateTime = DateTime.MinValue;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// get - Retrieve the current DateTime in an optimized method.
        /// </summary>
        public static DateTime Now
        {
            get
            {
                var tick_count = Environment.TickCount;
                if (tick_count == _LastTicks)
                    return _LastDateTime;

                var date = DateTime.Now;

                _LastTicks = tick_count;
                _LastDateTime = date;
                return date;
            }
        }

        /// <summary>
        /// get - Retrieve the current DateTime in an optimized method.
        /// </summary>
        public static DateTime UtcNow
        {
            get
            {
                var tick_count = Environment.TickCount;
                if (tick_count == _LastTicks)
                    return _LastDateTime;

                var date = DateTime.UtcNow;

                _LastTicks = tick_count;
                _LastDateTime = date;
                return date;
            }
        }
        #endregion

        #region Events
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.SharedKernel
{
    /// <summary>
    /// Custom class that allows us to set the date for testing. 
    /// Prefer this way as opposed to injecting an interface for the clock
    /// so I don't have to pass that dependency around.
    /// </summary>
    public static class SystemClock
    {
        private static DateTime? _customDate;

        public static DateTime Now
        {
            get
            {
                if (_customDate.HasValue)
                {
                    return _customDate.Value;
                }

                return DateTime.UtcNow;
            }
        }

        public static void Set(DateTime customDate) => _customDate = customDate;

        public static void Reset() => _customDate = null;
    }
}

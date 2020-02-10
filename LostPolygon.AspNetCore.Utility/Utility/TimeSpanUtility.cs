using System;

namespace LostPolygon.AspNetCore.Utility {
    public static class TimeSpanUtility {
        public static string FormatTimeZoneOffset(TimeSpan timeSpan) {
            return String.Format("{0}{1:00}:{2:00}", timeSpan.Hours >= 0 ? "+" : "−", Math.Abs(timeSpan.Hours), timeSpan.Minutes);
        }
    }
}

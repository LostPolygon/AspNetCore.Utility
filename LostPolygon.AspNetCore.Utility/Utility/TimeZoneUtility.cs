using System.Runtime.InteropServices;
using TimeZoneConverter;

namespace Ballast.Atlantis.Utility {
    public static class TimeZoneUtility {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static string TimeZoneWindowsToIana(string windowsTimeZoneId) {
            // Windows uses its own timezone format so we need to convert between them,
            // other OS's only uses IANA, so use the data as-is for those
            if (IsWindows)
                return TZConvert.WindowsToIana(windowsTimeZoneId, "001");

            return windowsTimeZoneId;
        }
    }
}

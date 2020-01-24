using System;

namespace LostPolygon.AspNetCore.Utility {
    public static class EnumUtility {
        public static TEnum? TryParseNullable<TEnum>(string value) where TEnum : struct, Enum {
            if (!Enum.TryParse(value, out TEnum result))
                return null;

            return result;
        }
    }
}

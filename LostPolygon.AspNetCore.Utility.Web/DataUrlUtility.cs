using System;

namespace LostPolygon.AspNetCore.Utility.Web {
    public static class DataUrlUtility {
        public static string CreateDataUrl(string mimeType, byte[] data) {
            return String.Format(
                "data:{0};base64,{1}",
                mimeType,
                Convert.ToBase64String(data)
            );
        }
    }
}

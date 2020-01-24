using System;
using Microsoft.Extensions.Hosting;

namespace LostPolygon.AspNetCore.Utility {
    public static class EnvironmentExtensions {
        private static readonly bool _isLocalDevelopment = Environment.GetEnvironmentVariable("LOCAL_DEVELOPMENT") != null;

        public static bool IsLocalDevelopment(this IHostEnvironment env) {
            return _isLocalDevelopment;
        }
    }
}

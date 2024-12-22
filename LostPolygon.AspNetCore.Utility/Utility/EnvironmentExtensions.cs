using System;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace LostPolygon.AspNetCore.Utility;

public static class EnvironmentExtensions {
    private static readonly bool IsLocalDevelopmentValue =
        Debugger.IsAttached ||
        Environment.GetEnvironmentVariable("LOCAL_DEVELOPMENT") != null ;

    public static bool IsLocalDevelopment(this IHostEnvironment env) => IsLocalDevelopmentValue;
}

using System;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace LostPolygon.AspNetCore.Utility;

public static class ConfigurationExtensions {
    public static T GetAndValidate<T, TValidator>(this IConfigurationSection section, Action<T>? beforeValidate = null)
        where TValidator : BaseOptionsValidator<T>, new() where T : class, new() {
        T config = section.Get<T>() ?? new T();
        beforeValidate?.Invoke(config);
        TValidator validator = new TValidator();
        validator.ValidateAndThrow(config);

        return config;
    }
}

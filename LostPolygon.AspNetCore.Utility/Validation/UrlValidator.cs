using System;
using FluentValidation.Validators;

namespace LostPolygon.AspNetCore.Utility;

public class UrlValidator : PropertyValidator {
    public UrlValidator()
        : base("{PropertyName} must be a valid URL.") {
    }

    protected override bool IsValid(PropertyValidatorContext context) {
        string? valueString = context.PropertyValue as string;
        if (String.IsNullOrWhiteSpace(valueString))
            return false;

        return Uri.TryCreate(valueString, UriKind.Absolute, out Uri? _);
    }
}

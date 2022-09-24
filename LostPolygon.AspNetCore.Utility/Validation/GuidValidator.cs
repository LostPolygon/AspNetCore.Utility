using System;
using FluentValidation.Validators;

namespace LostPolygon.AspNetCore.Utility;

public class GuidValidator : PropertyValidator {
    public string GuidFormat { get; }

    public GuidValidator(string guidFormat = "D")
        : base("{PropertyName} must be a valid GUID.") {
        GuidFormat = guidFormat;
    }

    protected override bool IsValid(PropertyValidatorContext context) {
        return context.PropertyValue is string valueString && Guid.TryParseExact(valueString, GuidFormat, out _);
    }
}

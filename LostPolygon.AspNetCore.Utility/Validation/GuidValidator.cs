using System;
using FluentValidation.Validators;

namespace LostPolygon.AspNetCore.Utility; 

public class GuidValidator : PropertyValidator {
    public GuidValidator()
        : base("{PropertyName} must be a valid GUID.") {
    }

    protected override bool IsValid(PropertyValidatorContext context) {
        return context.PropertyValue is string valueString && Guid.TryParseExact(valueString, "D", out _);
    }
}
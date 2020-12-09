using System;
using System.ComponentModel.DataAnnotations;

namespace LostPolygon.AspNetCore.Utility {
    public class ValidGuidAttribute : ValidationAttribute {
        public ValidGuidAttribute()
            : base("The {0} field must be a valid GUID.") {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            string? valueString = value as string;
            if (valueString == null || !Guid.TryParseExact(valueString, "D", out _))
                return new ValidationResult(
                    String.Format(ErrorMessageString, validationContext.DisplayName),
                    new[] { validationContext.MemberName! }
                );

            return ValidationResult.Success;
        }
    }
}

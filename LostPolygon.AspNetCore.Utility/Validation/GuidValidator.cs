using System;
using FluentValidation.Validators;

namespace Ballast.Atlantis.Utility {
    public class GuidValidator : PropertyValidator {
        public GuidValidator()
            : base("{PropertyName} must be a valid GUID.") {
        }

        protected override bool IsValid(PropertyValidatorContext context) {
            string? valueString = context.PropertyValue as string;
            if (valueString == null || !Guid.TryParseExact(valueString, "D", out _))
                return false;

            return true;
        }
    }
}

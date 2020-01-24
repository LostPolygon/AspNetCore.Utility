using System.Linq;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace LostPolygon.AspNetCore.Utility {
    public abstract class BaseOptionsValidator<T> : BaseValidator<T>, IValidateOptions<T> where T : class {
        public ValidateOptionsResult Validate(string name, T options) {
            ValidationResult validationResult = Validate(options);
            if (validationResult.IsValid)
                return ValidateOptionsResult.Success;

            return ValidateOptionsResult.Fail(validationResult.Errors.Select(failure => $"[{failure.PropertyName}]: {failure.ErrorMessage}"));
        }
    }
}

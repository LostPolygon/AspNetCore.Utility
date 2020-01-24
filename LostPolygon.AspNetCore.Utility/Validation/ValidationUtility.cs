using System;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace LostPolygon.AspNetCore.Utility {
    public static class ValidationUtility {
        public static IdentityResult ToIdentityResult(this ValidationResult validationResult) {
            IdentityResult result =
                validationResult.IsValid ?
                    IdentityResult.Success :
                    IdentityResult.Failed(
                        validationResult.Errors
                            .Select(e => new IdentityError {
                                Description = e.ErrorMessage,
                                Code = e.ErrorCode
                            })
                            .ToArray()
                    );

            return result;
        }

        public static bool ValidateSymmetricSecurityKey(string base64Key, int length) {
            try {
                byte[] key = Convert.FromBase64String(base64Key);
                return key.Length == length;
            } catch {
                return false;
            }
        }
    }
}

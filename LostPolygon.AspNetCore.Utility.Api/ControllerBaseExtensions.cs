using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ballast.Atlantis.Utility {
    public static class ControllerBaseExtensions {
        public static UnauthorizedObjectResult UnauthorizedApiProblemDetails(
            this ControllerBase controller,
            IDescriptiveError? error = null,
            ModelStateDictionary? modelState = null) {
            return ApiProblemDetailsUtility.UnauthorizedApiProblemDetails(
                error,
                modelState ?? controller.ModelState
            );
        }

        public static UnprocessableEntityObjectResult UnprocessableEntityApiProblemDetails(
            this ControllerBase controller,
            IDescriptiveError? error = null,
            ModelStateDictionary? modelState = null) {
            return ApiProblemDetailsUtility.UnprocessableEntityApiProblemDetails(
                error,
                modelState ?? controller.ModelState
            );
        }
    }
}

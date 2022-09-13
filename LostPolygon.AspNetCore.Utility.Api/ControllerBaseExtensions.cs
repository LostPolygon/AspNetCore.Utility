using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LostPolygon.AspNetCore.Utility.Api;

public static class ControllerBaseExtensions {
    public static UnauthorizedObjectResult UnauthorizedApiProblemDetails(
        this ControllerBase controller,
        IDescriptiveError? error = null,
        ModelStateDictionary? modelState = null
    ) {
        return ApiProblemDetailsUtility.ApiProblemDetails(
            details => new UnauthorizedObjectResult(details),
            HttpStatusCode.Unauthorized,
            error,
            modelState ?? controller.ModelState
        );
    }

    public static UnprocessableEntityObjectResult UnprocessableEntityApiProblemDetails(
        this ControllerBase controller,
        IDescriptiveError? error = null,
        ModelStateDictionary? modelState = null) {
        return ApiProblemDetailsUtility.ApiProblemDetails(
            details => new UnprocessableEntityObjectResult(details),
            HttpStatusCode.UnprocessableEntity,
            error,
            modelState ?? controller.ModelState
        );
    }

    public static NotFoundObjectResult NotFoundApiProblemDetails(
        this ControllerBase controller,
        IDescriptiveError? error = null,
        ModelStateDictionary? modelState = null
    ) {
        return ApiProblemDetailsUtility.ApiProblemDetails(
            details => new NotFoundObjectResult(details),
            HttpStatusCode.NotFound,
            error,
            modelState ?? controller.ModelState
        );
    }
}

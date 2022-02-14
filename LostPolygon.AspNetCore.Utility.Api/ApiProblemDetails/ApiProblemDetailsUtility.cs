using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LostPolygon.AspNetCore.Utility.Api;

public static class ApiProblemDetailsUtility {
    public static UnauthorizedObjectResult UnauthorizedApiProblemDetails(
        IDescriptiveError? error = null,
        ModelStateDictionary? modelState = null,
        IReadOnlyDictionary<string, string[]>? additionalInfo = null) {
        return ApiProblemDetails(
            details => new UnauthorizedObjectResult(details),
            HttpStatusCode.Unauthorized,
            error,
            modelState,
            additionalInfo
        );
    }

    public static UnprocessableEntityObjectResult UnprocessableEntityApiProblemDetails(
        IDescriptiveError? error = null,
        ModelStateDictionary? modelState = null,
        IReadOnlyDictionary<string, string[]>? additionalInfo = null) {
        return ApiProblemDetails(
            details => new UnprocessableEntityObjectResult(details),
            HttpStatusCode.UnprocessableEntity,
            error,
            modelState,
            additionalInfo
        );
    }

    public static T ApiProblemDetails<T>(
        Func<ApiProblemDetails, T> objectResultFactory,
        HttpStatusCode statusCode,
        IDescriptiveError? error = null,
        ModelStateDictionary? modelState = null,
        IReadOnlyDictionary<string, string[]>? additionalInfo = null)
        where T : ObjectResult {
        if (modelState == null && error == null)
            throw new ArgumentException(
                $"At least one of '{nameof(modelState)}' and " +
                $"{nameof(error)} parameters must be non-null");

        modelState ??= new ModelStateDictionary();

        if (error != null) {
            modelState.AddModelError(error);
        }

        ApiProblemDetails problemDetails = new ApiProblemDetails(
            modelState,
            statusCode: statusCode,
            additionalInfo: additionalInfo
        );
        T result = objectResultFactory(problemDetails);
        result.ContentTypes.Clear();
        result.ContentTypes.Add("application/problem+json");
        return result;
    }

    public static ModelStateDictionary ToModelStateDictionary(this IEnumerable<IdentityError> identityErrors) {
        ModelStateDictionary modelStateDictionary = new();
        foreach (IdentityError identityError in identityErrors) {
            modelStateDictionary.TryAddModelError(identityError.Code, identityError.Description);
        }

        return modelStateDictionary;
    }
}

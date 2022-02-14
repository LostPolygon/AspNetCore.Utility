using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LostPolygon.AspNetCore.Utility.Api;

public class ApiProblemDetails {
    [JsonPropertyName("status")]
    public int StatusCode { get; }

    [JsonPropertyName("title")]
    public string? Message { get; }

    [JsonPropertyName("errors")]
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    [JsonPropertyName("additionalInfo")]
    public IReadOnlyDictionary<string, string[]>? AdditionalInfo { get; }

    public ApiProblemDetails(
        ModelStateDictionary modelState,
        HttpStatusCode statusCode,
        string? message = null,
        IReadOnlyDictionary<string, string[]>? additionalInfo = null
    ) {
        Message = message;
        StatusCode = (int) statusCode;
        AdditionalInfo = additionalInfo;

        Dictionary<string, string[]> errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        foreach (KeyValuePair<string, ModelStateEntry> keyModelStatePair in modelState) {
            string key = keyModelStatePair.Key;
            ModelErrorCollection errorCollection = keyModelStatePair.Value.Errors;
            if (errorCollection != null && errorCollection.Count != 0) {
                List<string> errorStrings = errorCollection.Select(GetErrorMessage).ToList();
                if (keyModelStatePair.Value.AttemptedValue != null) {
                    errorStrings.Add(keyModelStatePair.Value.AttemptedValue);
                }

                errors.Add(key, errorStrings.ToArray());
            }
        }

        Errors = errors;

        static string GetErrorMessage(ModelError error) {
            return String.IsNullOrEmpty(error.ErrorMessage) ?
                "Unknown error." :
                error.ErrorMessage;
        }
    }
}

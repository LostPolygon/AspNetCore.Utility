using System.Net;

namespace LostPolygon.AspNetCore.Utility.ResultTypes;

public record struct Unauthorized;

public readonly record struct Unauthorized<T>(T? Value);

public record struct AlreadyExists;

public readonly record struct AlreadyExists<T>(T? Value);

public readonly record struct NotFound<T>(T? Value);

public readonly record struct HttpRequestError(HttpStatusCode StatusCode, string ReasonPhrase, string ResponseBody);

public readonly record struct PreconditionError<T>(T Value) {
    public static PreconditionError<T> Create(T value) => new(value);
}

public static class PreconditionError {
    public static PreconditionError<T> Create<T>(T value) => new(value);
    public static PreconditionError<DescriptiveError> CreateFromDescriptiveError(IDescriptiveError error) => new(new DescriptiveError(error));
}

public readonly record struct NonRetryableError<T>(T Value);

public static class NonRetryableError {
    public static NonRetryableError<T> Create<T>(T value) => new(value);
    public static NonRetryableError<DescriptiveError> CreateFromDescriptiveError(IDescriptiveError error) => new(new DescriptiveError(error));
}

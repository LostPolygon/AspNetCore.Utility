using System.Net;

namespace LostPolygon.AspNetCore.Utility.ResultTypes;

public record struct Unauthorized;
public readonly record struct Unauthorized<T>(T? Value);

public record struct AlreadyExists;
public readonly record struct AlreadyExists<T>(T? Value);

public readonly record struct NotFound<T>(T? Value);

public readonly record struct HttpRequestError(HttpStatusCode StatusCode, string ReasonPhrase, string ResponseBody);

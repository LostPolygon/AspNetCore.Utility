using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CompressedStaticFiles;

public class CompressedStaticFileMiddleware {
    private static Dictionary<string, string> compressionTypes =
        new Dictionary<string, string>() {
            { "gzip", ".gz" },
            { "br", ".br" }
        };

    private readonly IOptions<StaticFileOptions> _staticFileOptions;
    private readonly StaticFileMiddleware _base;
    private readonly ILogger _logger;

    public CompressedStaticFileMiddleware(
        RequestDelegate next, IWebHostEnvironment hostingEnv, IOptions<StaticFileOptions> staticFileOptions, ILoggerFactory loggerFactory) {
        if (next == null) {
            throw new ArgumentNullException(nameof(next));
        }

        if (loggerFactory == null) {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        if (hostingEnv == null) {
            throw new ArgumentNullException(nameof(hostingEnv));
        }

        _logger = loggerFactory.CreateLogger<CompressedStaticFileMiddleware>();

        this._staticFileOptions = staticFileOptions ?? throw new ArgumentNullException(nameof(staticFileOptions));
        InitializeStaticFileOptions(hostingEnv, staticFileOptions);

        _base = new StaticFileMiddleware(next, hostingEnv, staticFileOptions, loggerFactory);
    }

    private static void InitializeStaticFileOptions(IWebHostEnvironment hostingEnv, IOptions<StaticFileOptions> staticFileOptions) {
        staticFileOptions.Value.FileProvider ??= hostingEnv.WebRootFileProvider;
        IContentTypeProvider? contentTypeProvider = staticFileOptions.Value.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
        if (contentTypeProvider is FileExtensionContentTypeProvider fileExtensionContentTypeProvider) {
            // the StaticFileProvider would not serve the file if it does not know the content-type
            fileExtensionContentTypeProvider.Mappings[".br"] = "application/brotli";
        }

        staticFileOptions.Value.ContentTypeProvider = contentTypeProvider;

        Action<StaticFileResponseContext>? originalPrepareResponse = staticFileOptions.Value.OnPrepareResponse;
        staticFileOptions.Value.OnPrepareResponse = ctx => {
            originalPrepareResponse(ctx);
            foreach (string? compressionType in compressionTypes.Keys) {
                string? fileExtension = compressionTypes[compressionType];
                if (ctx.File.Name.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase)) {
                    // we need to restore the original content type, otherwise it would be based on the compression type
                    // (for example "application/brotli" instead of "text/html")
                    string? contentType = null;
                    string filePath = ctx.File.PhysicalPath!.Remove(ctx.File.PhysicalPath.Length - fileExtension.Length, fileExtension.Length);
                    if (contentTypeProvider.TryGetContentType(filePath, out contentType))
                        ctx.Context.Response.ContentType = contentType;

                    ctx.Context.Response.Headers["Content-Encoding"] = new[] { compressionType };
                }
            }
        };
    }

    public Task Invoke(HttpContext context) {
        if (context.Request.Path.HasValue) {
            ProcessRequest(context);
        }

        return _base.Invoke(context);
    }

    private void ProcessRequest(HttpContext context) {
        IFileProvider fileSystem = _staticFileOptions.Value.FileProvider!;
        IFileInfo? originalFile = fileSystem.GetFileInfo(context.Request.Path);

        if (!originalFile.Exists) {
            return;
        }

        IEnumerable<string>? supportedEncodings = GetSupportedEncodings(context);

        // try to find a compressed version of the file and ensure that it is smaller than the uncompressed version
        IFileInfo matchedFile = originalFile;
        foreach (string? compressionType in supportedEncodings) {
            string? fileExtension = compressionTypes[compressionType];
            IFileInfo? file = fileSystem.GetFileInfo(context.Request.Path + fileExtension);
            if (file.Exists && file.Length < matchedFile.Length) {
                matchedFile = file;
            }
        }

        if (matchedFile != originalFile) {
            // a compressed version exists and is smaller, change the path to serve the compressed file
            string? matchedPath = context.Request.Path.Value + Path.GetExtension(matchedFile.Name);
            //_logger.LogFileServed(context.Request.Path.Value, matchedPath, originalFile.Length, matchedFile.Length);
            context.Request.Path = new PathString(matchedPath);
        }
    }

    /// <summary>
    /// Find the encodings that are supported by the browser and by this middleware
    /// </summary>
    private static IEnumerable<string> GetSupportedEncodings(HttpContext context) {
        string[]? browserSupportedCompressionTypes = context.Request.Headers["Accept-Encoding"].ToString().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        IEnumerable<string>? validCompressionTypes = compressionTypes.Keys.Intersect(browserSupportedCompressionTypes, StringComparer.OrdinalIgnoreCase);
        return validCompressionTypes;
    }
}

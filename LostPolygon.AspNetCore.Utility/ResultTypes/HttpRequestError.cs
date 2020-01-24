using System.Net;

namespace Ballast.Atlantis.Utility.ResultTypes {
    public struct HttpRequestError {
        public HttpStatusCode StatusCode { get; }
        public string ReasonPhrase { get; }
        public string ResponseBody { get; }

        public HttpRequestError(HttpStatusCode statusCode, string reasonPhrase, string responseBody) {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            ResponseBody = responseBody;
        }

        public override string ToString() {
            return
                $"{StatusCode} {ReasonPhrase}, " +
                $"{nameof(ResponseBody)}: {ResponseBody}";
        }
    }
}

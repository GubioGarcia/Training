using System;
using System.Net;

namespace Template.CrossCutting.ExceptionHandler.Extensions
{
    public class ApiException : Exception
    {
        public ApiException(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
            string details = null,
            Exception innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Details = details;
            StackTraceInfo = innerException?.StackTrace;
        }

        public HttpStatusCode StatusCode { get; set; }
        public string Details { get; set; }
        public string StackTraceInfo { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace NetCoreReactTempl.Web.API.ApiExceptions
{
    public class ApiException
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public ApiException(IEnumerable<string> errors = null, string message = null, int statusCode = 500)
        {
            Message = message;
            StatusCode = statusCode;
            Errors = errors;
        }
        public ApiException(Exception ex, int statusCode = 500)
        {
            Message = ex.Message;
            StatusCode = statusCode;
        }
    }
}

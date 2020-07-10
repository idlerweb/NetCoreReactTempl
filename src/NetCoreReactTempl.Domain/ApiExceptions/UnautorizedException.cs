using System;
using System.Collections.Generic;

namespace NetCoreReactTempl.Domain.ApiExceptions
{
    public class UnauthorizedException : Exception
    {
        public IEnumerable<string> Errors { get; set; }
        public int StatusCode { get; set; }
        public UnauthorizedException(IEnumerable<string> errors = null,
                                    string message = "Unauthorized",
                                    int statusCode = 401) :
            base(message)
        {
            Errors = errors;
            StatusCode = statusCode;
        }
    }
}

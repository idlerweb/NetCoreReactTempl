using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreReactTempl.Domain.ApiExceptions;
using NetCoreReactTempl.Web.API.ApiExceptions;
using System.Linq;

namespace NetCoreReactTempl.Web.API.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ApiException apiException = null;
            if (context.Exception is ValidationException)
            {
                var ex = context.Exception as ValidationException;
                context.Exception = null;
                apiException = new ApiException(ex.Errors.Select(e => e.ErrorMessage), ex.Message, 400);

                context.HttpContext.Response.StatusCode = 400;
            }
            else if (context.Exception is UnauthorizedException)
            {
                var ex = context.Exception as UnauthorizedException;
                context.Exception = null;
                apiException = new ApiException(ex.Errors, ex.Message, 401);

                context.HttpContext.Response.StatusCode = 401;
            }
            else
            {
                apiException = new ApiException(null, context.Exception.Message, 500);
                context.Exception = null;
                context.HttpContext.Response.StatusCode = 500;
            }

            context.Result = new JsonResult(apiException);
            base.OnException(context);
        }
    }
}

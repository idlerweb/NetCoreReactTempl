using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace NetCoreReactTempl.Web.API.Controllers.Abstractions
{
    public abstract class CommonController<TContrller> : ControllerBase where TContrller : class
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger<TContrller> _logger;

        public CommonController(IMediator mediator, ILogger<TContrller> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected long UserId
        {
            get
            {
                if (long.TryParse(User.FindFirst(ClaimTypes.Name)?.Value, out var result))
                {
                    return result;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}

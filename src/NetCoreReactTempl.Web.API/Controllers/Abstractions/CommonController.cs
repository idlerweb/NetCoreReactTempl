using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace NetCoreReactTempl.Web.API.Controllers.Abstractions
{
    public abstract class CommonController<TContrller> : ControllerBase where TContrller : class
    {
        protected readonly IMediator Mediator;
        protected readonly IMapper Mapper;
        protected readonly ILogger<TContrller> Logger;

        public CommonController(IMediator mediator, IMapper mapper, ILogger<TContrller> logger)
        {
            Mediator = mediator;
            Mapper = mapper;
            Logger = logger;
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

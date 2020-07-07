using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreReactTempl.App;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Web.API.Controllers.Abstractions;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : RestController<AuthController, Dto.AuthInfo>
    {
        public AuthController(IMediator mediator, IMapper mapper, ILogger<AuthController> logger)
            : base(mediator, mapper, logger)
        {
        }

        [HttpGet]
        public override async Task<IActionResult> GetList(int top = 10, int skip = 0, string search = null) =>
            Ok(new RestResponseBase<AuthInfo>(
                list: await Mediator.Send(new App.Handlers.Auth.Query.GetList(0, UserId, null))
            ));

        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(long id) =>
            Ok(new RestResponseBase<AuthInfo>(
                data: await Mediator.Send(new App.Handlers.Auth.Query.Get(id, UserId, null))
            ));

        [HttpPost]
        public override async Task<IActionResult> Post(Dto.AuthInfo dto) =>
            Ok(new RestResponseBase<AuthInfo>(
                data: await Mediator.Send(new App.Handlers.Auth.Command.Registration(dto.Id, UserId, Mapper.Map<AuthInfo>(dto)))
            ));

        [HttpPut]
        public override async Task<IActionResult> Put(Dto.AuthInfo dto) => 
            Ok(new RestResponseBase<AuthInfo>(
                data: await Mediator.Send(new App.Handlers.Auth.Command.Authorization(dto.Id, UserId, Mapper.Map<AuthInfo>(dto)))
            ));

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(long id) => 
            Ok(new RestResponseBase<BaseModel>(
                data: await Mediator.Send(new App.Handlers.Auth.Command.Delete(id, UserId, null))
            ));
    }
}

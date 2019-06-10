using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreReactTempl.Web.API.Controllers.Abstractions;
using NetCoreReactTempl.Web.API.Handlers.Auth;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : RestController<AuthController, Dto.AuthInfo>
    {
        public AuthController(IMediator mediator, ILogger<AuthController> logger)
            : base(mediator, logger) { }

        [HttpGet]
        public override async Task<IActionResult> GetList(int top = 10, int skip = 0, string search = null)
        {
            return Ok(await _mediator.Send(new Handlers.Auth.Query.GetList(0, UserId, null)));
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(long id)
        {
            return Ok(await _mediator.Send(new Handlers.Auth.Query.Get(id, UserId, null)));
        }

        [HttpPost]
        public override async Task<IActionResult> Post(Dto.AuthInfo dto)
        {
            return Ok(await _mediator.Send(new Handlers.Auth.Command.Registration(dto.Id, UserId, dto)));
        }

        [HttpPut]
        public override async Task<IActionResult> Put(Dto.AuthInfo dto)
        {
            return Ok(await _mediator.Send(new Handlers.Auth.Command.Authorization(dto.Id, UserId, dto)));
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(long id)
        {
            return Ok(await _mediator.Send(new Handlers.Auth.Command.Delete(id, UserId, null)));
        }
    }
}

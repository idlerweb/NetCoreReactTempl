using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreReactTempl.Web.API.Controllers.Abstractions;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Controllers
{
    [Route("api/data")]
    [ApiController]
    public class DataController : RestController<DataController, Dto.Data>
    {
        public DataController(IMediator mediator, ILogger<DataController> logger)
            : base(mediator, logger) { }


        [HttpGet]
        public override async Task<IActionResult> GetList(int top = 10, int skip = 0,  string search = null)
        {
            return Ok(await _mediator.Send(new Handlers.Data.Query.GetList(0, UserId, null, top, skip, search)));
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(long id)
        {
            return Ok(await _mediator.Send(new Handlers.Data.Query.Get(id, UserId, null)));
        }

        [HttpPost]
        public override async Task<IActionResult> Post(Dto.Data dto)
        {
            return Ok(await _mediator.Send(new Handlers.Data.Command.Create(dto.Id, UserId, dto)));
        }

        [HttpPut]
        public override async Task<IActionResult> Put(Dto.Data dto)
        {
            return Ok(await _mediator.Send(new Handlers.Data.Command.Update(dto.Id, UserId, dto)));
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(long id)
        {
            return Ok(await _mediator.Send(new Handlers.Data.Command.Delete(id, UserId, null)));
        }
    }
}

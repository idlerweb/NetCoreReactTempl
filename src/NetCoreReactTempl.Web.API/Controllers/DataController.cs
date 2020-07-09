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
    [Route("api/data")]
    [ApiController]
    public class DataController : RestController<DataController, Dto.Data>
    {
        public DataController(IMediator mediator, IMapper mapper, ILogger<DataController> logger)
            : base(mediator, mapper, logger) { }


        [HttpGet]
        public override async Task<IActionResult> GetList(int top = 10, int skip = 0, string search = null) =>
            Ok(new RestResponseBase<Data>(
                list: await Mediator.Send(new App.Handlers.Data.Query.GetList(0, UserId, null, top, skip, search))
            ));

        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(long id) =>
            Ok(new RestResponseBase<Data>(
                data: await Mediator.Send(new App.Handlers.Data.Query.Get(id, UserId, null))
            ));

        [HttpPost]
        public override async Task<IActionResult> Post(Dto.Data dto) =>
            Ok(new RestResponseBase<Data>(
                data: await Mediator.Send(new App.Handlers.Data.Command.Create(dto.Id, UserId, Mapper.Map<Data>(dto)))
            ));

        [HttpPut]
        public override async Task<IActionResult> Put(Dto.Data dto) =>
            Ok(new RestResponseBase<Data>(
                data: await Mediator.Send(new App.Handlers.Data.Command.Update(dto.Id, UserId, Mapper.Map<Data>(dto)))
            ));

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(long id) =>
            Ok(new RestResponseBase<BaseData>(
                data: await Mediator.Send(new App.Handlers.Data.Command.Delete(id, UserId, null))
            ));
    }
}

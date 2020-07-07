using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Controllers.Abstractions
{
    public abstract class RestController<TController, TDto> : CommonController<TController> where TController : class
                                                                                                       where TDto : Dto.BaseDto
                                            
    {
        public RestController(IMediator mediator, IMapper mapper, ILogger<TController> logger)
            : base(mediator, mapper, logger)
        {
        }

        public abstract Task<IActionResult> GetList(int top = 10, int skip = 0, string search = null);
        public abstract Task<IActionResult> Get(long id);
        public abstract Task<IActionResult> Post(TDto dto);
        public abstract Task<IActionResult> Put(TDto dto);
        public abstract Task<IActionResult> Delete(long id);
    }
}

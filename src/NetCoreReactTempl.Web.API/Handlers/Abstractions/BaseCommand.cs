using MediatR;
using NetCoreReactTempl.Web.API.Dto;

namespace NetCoreReactTempl.Web.API.Handlers.Abstractions
{
    public abstract class BaseCommand<TResponse, TData> : IRequest<TResponse> where TResponse : BaseResponse<TData>
                                                                              where TData : BaseDto
    {
        public readonly long Id;
        public readonly long UserId;
        public readonly TData Data;

        public BaseCommand(long id, long userId, TData data)
        {
            Id = id;
            UserId = userId;
            Data = data;
        }
    }
}

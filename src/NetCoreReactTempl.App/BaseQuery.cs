using MediatR;
using NetCoreReactTempl.Domain.Models;

namespace NetCoreReactTempl.App
{
    public class BaseQuery<TData, TResponse> : IRequest<TResponse> where TResponse: class
                                                                   where TData : BaseModel
    {
        public readonly long Id;
        public readonly long UserId;
        public readonly TData Data;

        public BaseQuery(long id, long userId, TData data)
        {
            Id = id;
            UserId = userId;
            Data = data;
        }
    }
}

using MediatR;
using NetCoreReactTempl.Domain.Models;

namespace NetCoreReactTempl.App
{
    public abstract class BaseCommand<TData> : IRequest<TData> where TData : BaseData
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

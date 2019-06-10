using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Handlers.Data.Query
{
    public class GetList : BaseCommand<BaseResponse<Dto.Data>, Dto.Data>
    {
        public int Top { get; set; }
        public int Skip { get; set; }
        public string Search { get; set; }
        public GetList(long id, long userId, Dto.Data data, int top, int skip, string search)
            : base(id, userId, data)
        {
            Top = top;
            Skip = skip;
            Search = search;
        }
    }

    public class GetListHandler : IRequestHandler<GetList, BaseResponse<Dto.Data>>
    {
        private readonly IMapper _mapper;
        private readonly IDataManager<DAL.Entities.Data> _dataManager;

        public GetListHandler(IDataManager<DAL.Entities.Data> dataManager, IMapper mapper)
        {
            _dataManager = dataManager;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Dto.Data>> Handle(GetList query, CancellationToken cancellationToken)
        {
            var collection = _dataManager.GetCollection().Where(d => d.UserId == query.UserId);
            IQueryable<DAL.Entities.Data> result;

            if (query.Search != null)
            {
                result = collection.Where(d => d.Field1.Contains(query.Search));
            }
            else
            {
                result = collection;
            }

            return new BaseResponse<Dto.Data>(null,
                _mapper.Map<List<Dto.Data>>(await result.Skip(query.Skip).Take(query.Top).ToListAsync()),
                await collection.CountAsync());
        }
    }

    public class GetListValidator : AbstractValidator<GetList>
    {
        public GetListValidator()
        {
        }
    }
}


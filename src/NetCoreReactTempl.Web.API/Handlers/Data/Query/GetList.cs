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
        private readonly IDataManager<DAL.Entities.Data> _dataManager;
        private readonly IDataManager<DAL.Entities.Field> _fieldsManager;

        public GetListHandler(IDataManager<DAL.Entities.Data> dataManager, IDataManager<DAL.Entities.Field> fieldsManager)
        {
            _dataManager = dataManager;
            _fieldsManager = fieldsManager;
        }

        public async Task<BaseResponse<Dto.Data>> Handle(GetList query, CancellationToken cancellationToken)
        {
            if (query.Search != null)
            {
                var ids = _fieldsManager.GetCollection().Where(d => d.Value.Contains(query.Search)).Select(i => i.DataId).Distinct();
                var datas = _dataManager.GetCollection().Where(d => d.UserId == query.UserId && ids.Contains(d.Id));

                var list = datas.ToList().Select(d =>
                {
                    var fields = _fieldsManager.GetCollection().Where(f => f.DataId == d.Id).Select(f => new KeyValuePair<string, string>(f.Name, f.Value)).ToList();
                    return new Dto.Data()
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        Fields = fields.ToDictionary(a => a.Key, a => a.Value)
                    };
                }).ToList();
                return new BaseResponse<Dto.Data>(null, list, await datas.CountAsync());
            }
            else
            {
                var datas = _dataManager.GetCollection().Where(d => d.UserId == query.UserId);

                var list = datas.ToList().Select(d =>
                {
                    var fields = _fieldsManager.GetCollection().Where(f => f.DataId == d.Id).Select(f => new KeyValuePair<string, string>(f.Name, f.Value)).ToList();
                    return new Dto.Data()
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        Fields = fields.ToDictionary(a => a.Key, a => a.Value)
                    };
                }).ToList();
                return new BaseResponse<Dto.Data>(null, list, await datas.CountAsync());
            }
        }
    }

    public class GetListValidator : AbstractValidator<GetList>
    {
        public GetListValidator()
        {
        }
    }
}


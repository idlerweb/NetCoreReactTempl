using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.App.Handlers.Data.Query
{
    public class GetList : BaseQuery<Domain.Models.Data, IEnumerable<Domain.Models.Data>>
    {
        public int Top { get; set; }
        public int Skip { get; set; }
        public string Search { get; set; }
        public GetList(long id, long userId, Domain.Models.Data data, int top, int skip, string search)
            : base(id, userId, data)
        {
            Top = top;
            Skip = skip;
            Search = search;
        }
    }

    public class GetListHandler : IRequestHandler<GetList, IEnumerable<Domain.Models.Data>>
    {
        private readonly IDataManager<Domain.Models.Data> _dataManager;
        private readonly IDataManager<Domain.Models.Field> _fieldsManager;

        public GetListHandler(IDataManager<Domain.Models.Data> dataManager, IDataManager<Domain.Models.Field> fieldsManager)
        {
            _dataManager = dataManager;
            _fieldsManager = fieldsManager;
        }

        public async Task<IEnumerable<Domain.Models.Data>> Handle(GetList query, CancellationToken cancellationToken)
        {
            if (query.Search != null)
            {
                var ids = (await _fieldsManager.GetCollection()).Where(d => d.Value.Contains(query.Search)).Select(i => i.DataId).Distinct();
                var datas = (await _dataManager.GetCollection()).Where(d => d.UserId == query.UserId && ids.Contains(d.Id));

                var listTasks = datas.Select(async d =>
                {
                    var fields = (await _fieldsManager.GetCollection()).Where(f => f.DataId == d.Id).Select(f => new KeyValuePair<string, string>(f.Name, f.Value)).ToList();
                    return new Domain.Models.Data()
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        Fields = fields.ToDictionary(a => a.Key, a => a.Value as object)
                    };
                });

                await Task.WhenAll(listTasks);

                return listTasks.Select(r => r.Result).ToList();
            }
            else
            {
                var datas = (await _dataManager.GetCollection()).Where(d => d.UserId == query.UserId);

                var listTasks = datas.ToList().Select(async d =>
                {
                    var fields = (await _fieldsManager.GetCollection()).Where(f => f.DataId == d.Id).Select(f => new KeyValuePair<string, string>(f.Name, f.Value)).ToList();
                    return new Domain.Models.Data()
                    {
                        Id = d.Id,
                        UserId = d.UserId,
                        Fields = fields.ToDictionary(a => a.Key, a => a.Value as object)
                    };
                });

                await Task.WhenAll(listTasks);

                return listTasks.Select(r => r.Result).ToList();
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


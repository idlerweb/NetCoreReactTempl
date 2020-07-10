using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Repositories;
using System.Collections.Generic;
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

        public GetListHandler(IDataManager<Domain.Models.Data> dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<IEnumerable<Domain.Models.Data>> Handle(GetList query, CancellationToken cancellationToken) =>
            query switch
            {
                var q when string.IsNullOrEmpty(q.Search) => 
                    await _dataManager.GetUserData(query.UserId, query.Search),

                _ => await _dataManager.GetUserData(query.UserId),
            };
    }

    public class GetListValidator : AbstractValidator<GetList>
    {
        public GetListValidator()
        {
        }
    }
}


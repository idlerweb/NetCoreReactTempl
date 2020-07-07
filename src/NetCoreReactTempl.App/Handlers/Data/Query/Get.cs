using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.App.Handlers.Data.Query
{

    public class Get : BaseQuery<Domain.Models.Data, Domain.Models.Data>
    {
        public Get(long id, long userId, Domain.Models.Data data)
            : base(id, userId, data) { }
    }

    public class GetHandler : IRequestHandler<Get, Domain.Models.Data>
    {
        private readonly IDataManager<Domain.Models.Data> _dataManager;
        private readonly IDataManager<Domain.Models.Field> _fieldsManager;

        public GetHandler(IDataManager<Domain.Models.Data> dataManager, IDataManager<Domain.Models.Field> fieldsManager)
        {
            _dataManager = dataManager;
            _fieldsManager = fieldsManager;
        }

        public async Task<Domain.Models.Data> Handle(Get query, CancellationToken cancellationToken)
        {
            var data = await _dataManager.GetAsync(query.Id);
            var fields = (await _fieldsManager.GetCollection()).Where(f => f.DataId == data.Id)
                .Select(f => new KeyValuePair<string, string>(f.Name, f.Value));

            return new Domain.Models.Data()
            {
                Id = data.Id,
                UserId = data.UserId,
                Fields = fields.ToDictionary(a => a.Key, a => a.Value)
            };
        }
    }

    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id not empty");
        }
    }
}

using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Repositories;
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

        public GetHandler(IDataManager<Domain.Models.Data> dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<Domain.Models.Data> Handle(Get query, CancellationToken cancellationToken) =>
            await _dataManager.GetData(query.Id);
    }

    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id not empty");
        }
    }
}

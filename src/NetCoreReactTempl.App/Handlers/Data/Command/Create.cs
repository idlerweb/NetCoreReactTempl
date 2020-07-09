using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.App.Handlers.Data.Command
{

    public class Create : BaseCommand<Domain.Models.Data>
    {
        public Create(long id, long userId, Domain.Models.Data data)
            : base(id, userId, data) { }
    }

    public class CreateHandler : IRequestHandler<Create, Domain.Models.Data>
    {
        private readonly IDataManager<Domain.Models.Data> _dataManager;

        public CreateHandler(IDataManager<Domain.Models.Data> dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<Domain.Models.Data> Handle(Create command, CancellationToken cancellationToken)
        {
            var data = await _dataManager.Create(new Domain.Models.Data()
            {
                UserId = command.UserId,
                Fields = command.Data.Fields
            });

            return new Domain.Models.Data
            {
                Id = data.Id
            };
        }
    }

    public class CreateValidator : AbstractValidator<Create>
    {
        public CreateValidator()
        {
            RuleFor(c => c.Data).NotEmpty().WithMessage("Model not empty");
        }
    }
}

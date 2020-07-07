using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.App.Handlers.Data.Command
{

    public class Delete : BaseCommand<BaseModel>
    {
        public Delete(long id, long userId, Domain.Models.Data data)
            : base(id, userId, data) { }
    }

    public class DeleteHandler : IRequestHandler<Delete, BaseModel>
    {
        private readonly IDataManager<Domain.Models.Data> _dataManager;

        public DeleteHandler(IDataManager<Domain.Models.Data> dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<BaseModel> Handle(Delete command, CancellationToken cancellationToken)
        {
            await _dataManager.DeleteAsync(command.Id);
            return null;
        }
    }

    public class DeleteValidator : AbstractValidator<Delete>
    {
        public DeleteValidator(IDataManager<Domain.Models.Data> dataManager)
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Model not empty");
            RuleFor(c => c).MustAsync(async (c, ct) => (await dataManager.GetAsync(c.Id)).UserId == c.UserId).WithMessage("You not autor");
        }
    }
}


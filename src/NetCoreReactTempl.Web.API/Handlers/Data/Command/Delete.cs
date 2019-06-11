using FluentValidation;
using MediatR;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Handlers.Data.Command
{

    public class Delete : BaseCommand<BaseResponse<Dto.Data>, Dto.Data>
    {
        public Delete(long id, long userId, Dto.Data data)
            : base(id, userId, data) { }
    }

    public class DeleteHandler : IRequestHandler<Delete, BaseResponse<Dto.Data>>
    {
        private readonly IDataManager<DAL.Entities.Data> _dataManager;

        public DeleteHandler(IDataManager<DAL.Entities.Data> dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<BaseResponse<Dto.Data>> Handle(Delete command, CancellationToken cancellationToken)
        {
            await _dataManager.DeleteAsync(command.Id);
            return new BaseResponse<Dto.Data>(null, null, 0);
        }
    }

    public class DeleteValidator : AbstractValidator<Delete>
    {
        public DeleteValidator(IDataManager<DAL.Entities.Data> dataManager)
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Model not empty");
            RuleFor(c => c).MustAsync(async (c, ct) => (await dataManager.GetAsync(c.Id)).UserId == c.UserId).WithMessage("You not autor");
        }
    }
}


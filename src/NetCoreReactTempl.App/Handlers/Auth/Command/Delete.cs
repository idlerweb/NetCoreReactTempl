using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain;
using NetCoreReactTempl.Domain.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.App.Handlers.Auth.Command
{
    public class Delete : BaseCommand<Domain.Models.BaseModel>
    {
        public Delete(long id, long userId, Domain.Models.AuthInfo data)
            : base(id, userId, data) { }
    }

    public class DeleteHandler : IRequestHandler<Delete, Domain.Models.BaseModel>
    {
        private readonly IDataManager<Domain.Models.User> _userManager;

        public DeleteHandler(IDataManager<Domain.Models.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Domain.Models.BaseModel> Handle(Delete command, CancellationToken cancellationToken)
        {
            await _userManager.DeleteAsync(command.Id);
            return null;
        }
    }

    public class DeleteValidator : AbstractValidator<Delete>
    {
        public DeleteValidator(IDataManager<Domain.Models.User> userManager)
        {
            RuleFor(c => c.UserId)
                .MustAsync(async (c, ct) => (await userManager.GetCollection()).FirstOrDefault(u => u.Id == c)?.IsAdmin ?? false)
                .WithMessage("No admin");
        }
    }
}

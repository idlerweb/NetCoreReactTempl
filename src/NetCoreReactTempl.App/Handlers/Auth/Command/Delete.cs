using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.App.Handlers.Auth.Command
{
    public class Delete : BaseCommand<Domain.Models.BaseData>
    {
        public Delete(long id, long userId, Domain.Models.AuthInfo data)
            : base(id, userId, data) { }
    }

    public class DeleteHandler : IRequestHandler<Delete, Domain.Models.BaseData>
    {
        private readonly IAuthManager _authManager;

        public DeleteHandler(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        public async Task<Domain.Models.BaseData> Handle(Delete command, CancellationToken cancellationToken)
        {
            await _authManager.Delete(command.Id);
            return null;
        }
    }

    public class DeleteValidator : AbstractValidator<Delete>
    {
        public DeleteValidator(IAuthManager authManager)
        {
            RuleFor(c => c.UserId)
                .MustAsync(async (c, ct) => (await authManager.Get(c))?.IsAdmin ?? false)
                .WithMessage("No admin");
        }
    }
}

using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.App.Handlers.Auth.Query
{
    public class Get : BaseQuery<AuthInfo, AuthInfo>
    {
        public Get(long id, long userId, AuthInfo data)
            : base(id, userId, data) { }
    }

    public class GetHandler : IRequestHandler<Get, AuthInfo>
    {
        private readonly IAuthManager _authManager;

        public GetHandler(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        public async Task<AuthInfo> Handle(Get command, CancellationToken cancellationToken)
        {
            var createdUser = await _authManager.Get(command.Id);

            return new AuthInfo()
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                IsVerify = createdUser.IsVerify,
                Token = null
            };
        }
    }

    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator(IAuthManager authManager)
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id not empty");
            RuleFor(c => c.UserId)
                .MustAsync(async (c, ct) => (await authManager.Get(c))?.IsAdmin ?? false)
                .WithMessage("No admin");
        }
    }
}

using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Domain.Repositories;
using System.Linq;
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
        private readonly IDataManager<User> _userManager;

        public GetHandler(IDataManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthInfo> Handle(Get command, CancellationToken cancellationToken)
        {
            var createdUser = await _userManager.GetAsync(command.Id);

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
        public GetValidator(IDataManager<User> userManager)
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id not empty");
            RuleFor(c => c.UserId)
                .MustAsync(async (c, ct) => (await userManager.GetCollection()).FirstOrDefault(u => u.Id == c)?.IsAdmin ?? false)
                .WithMessage("No admin");
        }
    }
}

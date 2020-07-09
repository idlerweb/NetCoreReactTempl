using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.App.Handlers.Auth.Query
{
    public class GetList : BaseQuery<AuthInfo, IEnumerable<AuthInfo>>
    {
        public GetList(long id, long userId, AuthInfo data)
            : base(id, userId, data) { }
    }

    public class GetListHandler : IRequestHandler<GetList, IEnumerable<AuthInfo>>
    {
        private readonly IAuthManager _authManager;

        public GetListHandler(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        public async Task<IEnumerable<AuthInfo>> Handle(GetList command, CancellationToken cancellationToken)
        {
            var users = await _authManager.Get();
            return users.Select(u => new AuthInfo()
            {
                Id = u.Id,
                Email = u.Email,
                IsVerify = u.IsVerify,
                Token = null
            });
        }
    }

    public class GetListValidator : AbstractValidator<GetList>
    {
        public GetListValidator(IAuthManager authManager)
        {
            RuleFor(c => c.UserId)
                .MustAsync(async (c, ct) => (await authManager.Get(c))?.IsAdmin ?? false)
                .WithMessage("No admin");
        }
    }
}

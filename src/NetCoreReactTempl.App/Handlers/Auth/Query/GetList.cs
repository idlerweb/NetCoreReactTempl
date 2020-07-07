using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain;
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
        private readonly IDataManager<User> _userManager;

        public GetListHandler(IDataManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<AuthInfo>> Handle(GetList command, CancellationToken cancellationToken)
        {
            var users = await _userManager.GetCollection();
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
        public GetListValidator(IDataManager<User> userManager)
        {
            RuleFor(c => c.UserId)
                .MustAsync(async (c, ct) => (await userManager.GetCollection()).FirstOrDefault(u => u.Id == c)?.IsAdmin ?? false)
                .WithMessage("No admin");
        }
    }
}

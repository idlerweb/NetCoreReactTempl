using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.DAL.Entities;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Handlers.Auth.Query
{
    public class GetList : BaseCommand<BaseResponse<Dto.AuthInfo>, Dto.AuthInfo>
    {
        public GetList(long id, long userId, Dto.AuthInfo data)
            : base(id, userId, data) { }
    }

    public class GetListHandler : IRequestHandler<GetList, BaseResponse<Dto.AuthInfo>>
    {
        private readonly IDataManager<User> _userManager;

        public GetListHandler(IDataManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResponse<Dto.AuthInfo>> Handle(GetList command, CancellationToken cancellationToken)
        {
            var users = _userManager.GetCollection();
            return new BaseResponse<Dto.AuthInfo>(null, (await users.ToListAsync()).Select(u => new Dto.AuthInfo()
                {
                    Id = u.Id,
                    Email = u.Email,
                    IsVerify = u.IsVerify,
                    Token = null
                }).ToList(), await users.CountAsync());
        }
    }

    public class GetListValidator : AbstractValidator<GetList>
    {
        public GetListValidator(IDataManager<User> userManager)
        {
            RuleFor(c => c.UserId)
                .MustAsync(async (c, ct) => (await userManager.GetCollection().FirstAsync(u => u.Id == c)).IsAdmin)
                .WithMessage("No admin");
        }
    }
}

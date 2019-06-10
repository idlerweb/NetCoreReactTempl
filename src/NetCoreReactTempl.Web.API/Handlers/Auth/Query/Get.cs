using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.DAL.Entities;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Handlers.Auth.Query
{
    public class Get : BaseCommand<BaseResponse<Dto.AuthInfo>, Dto.AuthInfo>
    {
        public Get(long id, long userId, Dto.AuthInfo data)
            : base(id, userId, data) { }
    }

    public class GetHandler : IRequestHandler<Get, BaseResponse<Dto.AuthInfo>>
    {
        private readonly IDataManager<User> _userManager;

        public GetHandler(IDataManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResponse<Dto.AuthInfo>> Handle(Get command, CancellationToken cancellationToken)
        {
            var createdUser = await _userManager.GetAsync(command.Id);

            return new BaseResponse<Dto.AuthInfo>(new Dto.AuthInfo()
                {
                    Id = createdUser.Id,
                    Email = createdUser.Email,
                    IsVerify = createdUser.IsVerify,
                    Token = null
                }, null, 0);
        }
    }

    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator(IDataManager<User> userManager)
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id not empty");
            RuleFor(c => c.UserId)
                .MustAsync(async (c, ct) => (await userManager.GetCollection().FirstAsync(u => u.Id == c)).IsAdmin)
                .WithMessage("No admin");
        }
    }
}

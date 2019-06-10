using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.DAL.Entities;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Handlers.Auth.Command
{
    public class Delete : BaseCommand<BaseResponse<Dto.AuthInfo>, Dto.AuthInfo>
    {
        public Delete(long id, long userId, Dto.AuthInfo data)
            : base(id, userId, data) { }
    }

    public class DeleteHandler : IRequestHandler<Delete, BaseResponse<Dto.AuthInfo>>
    {
        private readonly IDataManager<User> _userManager;

        public DeleteHandler(IDataManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResponse<Dto.AuthInfo>> Handle(Delete command, CancellationToken cancellationToken)
        {
            await _userManager.DeleteAsync(command.Id);
            return new BaseResponse<Dto.AuthInfo>(null, null, 0);
        }
    }

    public class DeleteValidator : AbstractValidator<Delete>
    {
        public DeleteValidator(IDataManager<User> userManager)
        {
            RuleFor(c => c.UserId)
                .MustAsync(async (c, ct) => (await userManager.GetCollection().FirstAsync(u => u.Id == c)).IsAdmin)
                .WithMessage("No admin");
        }
    }
}

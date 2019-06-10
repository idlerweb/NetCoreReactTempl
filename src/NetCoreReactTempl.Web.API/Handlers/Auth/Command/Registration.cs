using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCoreReactTempl.DAL.Entities;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.Web.API.Handlers.Auth.Command
{
    public class Registration : BaseCommand<BaseResponse<Dto.AuthInfo>, Dto.AuthInfo>
    {
        public Registration(long id, long userId, Dto.AuthInfo data)
            : base(id, userId, data) { }
    }

    public class RegistrationHandler : IRequestHandler<Registration, BaseResponse<Dto.AuthInfo>>
    {
        private readonly IDataManager<User> _userManager;

        public RegistrationHandler(IDataManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResponse<Dto.AuthInfo>> Handle(Registration command, CancellationToken cancellationToken)
        {
            var user = new User() { Email = command.Data.Email };
            CreatePasswordHash(command.Data.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var createdUser = await _userManager.CreateAsync(user);

            return new BaseResponse<Dto.AuthInfo>(new Dto.AuthInfo()
                {
                    Id = createdUser.Id,
                    Email = createdUser.Email,
                    IsVerify = createdUser.IsVerify,
                    Token = null
                }, null, 0);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }

    public class RegistrationValidator : AbstractValidator<Registration>
    {

        public RegistrationValidator(IDataManager<User> userManager)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            RuleFor(c => c.Data.Email).NotEmpty().Must(c => regex.IsMatch(c)).WithMessage("Email not valid");
            RuleFor(c => c.Data.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(c => c.Data.Email)
                .MustAsync(async (email, ct) =>
                !(await userManager.GetCollection().AnyAsync(u => u.Email == email)))
                .WithMessage("Email Exist");
        }
    }
}

using FluentValidation;
using MediatR;
using NetCoreReactTempl.Domain.Repositories;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreReactTempl.App.Handlers.Auth.Command
{
    public class Registration : BaseCommand<Domain.Models.AuthInfo>
    {
        public Registration(long id, long userId, Domain.Models.AuthInfo data)
            : base(id, userId, data) { }
    }

    public class RegistrationHandler : IRequestHandler<Registration, Domain.Models.AuthInfo>
    {
        private readonly IDataManager<Domain.Models.User> _userManager;

        public RegistrationHandler(IDataManager<Domain.Models.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Domain.Models.AuthInfo> Handle(Registration command, CancellationToken cancellationToken)
        {
            var user = new Domain.Models.User() { Email = command.Data.Email };
            CreatePasswordHash(command.Data.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var createdUser = await _userManager.CreateAsync(user);

            return new Domain.Models.AuthInfo()
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                IsVerify = createdUser.IsVerify,
                Token = null
            };
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    public class RegistrationValidator : AbstractValidator<Registration>
    {

        public RegistrationValidator(IDataManager<Domain.Models.User> userManager)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            RuleFor(c => c.Data.Email).NotEmpty().Must(c => regex.IsMatch(c)).WithMessage("Email not valid");
            RuleFor(c => c.Data.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(c => c.Data.Email)
                .MustAsync(async (email, ct) => !(await userManager.GetCollection()).Any(u => u.Email == email))
                .WithMessage("Email Exist");
        }
    }
}

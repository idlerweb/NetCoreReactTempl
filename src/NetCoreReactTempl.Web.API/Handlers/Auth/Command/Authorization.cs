using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetCoreReactTempl.DAL.Entities;
using NetCoreReactTempl.DAL.Interfaces;
using NetCoreReactTempl.Web.API.ApiExceptions;
using NetCoreReactTempl.Web.API.Handlers.Abstractions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace NetCoreReactTempl.Web.API.Handlers.Auth.Command
{
    public class Authorization : BaseCommand<BaseResponse<Dto.AuthInfo>, Dto.AuthInfo>
    {
        public Authorization(long id, long userId, Dto.AuthInfo data)
            : base(id, userId, data) { }
    }

    public class AuthorizationHandler : IRequestHandler<Authorization, BaseResponse<Dto.AuthInfo>>
    {
        private readonly IDataManager<User> _userManager;
        private readonly ConfigurationStore _configurationStore;

        public AuthorizationHandler(IDataManager<User> userManager, ConfigurationStore configurationStore)
        {
            _userManager = userManager;
            _configurationStore = configurationStore;
        }

        public async Task<BaseResponse<Dto.AuthInfo>> Handle(Authorization command, CancellationToken cancellationToken)
        {
            var user = await AuthenticateAsync(command.Data.Email, command.Data.Password);

            if (user == null)
                throw new UnauthorizedException(new[] { "Login or password not valid" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configurationStore.AuthSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new BaseResponse<Dto.AuthInfo>(new Dto.AuthInfo()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsVerify = user.IsVerify,
                    Token = tokenString
                }, null, 0);
        }

        private async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userManager.GetCollection().FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }

    public class AuthorizationValidator : AbstractValidator<Authorization>
    {
        public AuthorizationValidator()
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            RuleFor(c => c.Data.Email).NotEmpty().Must(c => regex.IsMatch(c)).WithMessage("Email not valid");
            RuleFor(c => c.Data.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}

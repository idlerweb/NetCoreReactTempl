using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using NetCoreReactTempl.Domain.ApiExceptions;
using NetCoreReactTempl.Domain.Configuration;
using NetCoreReactTempl.Domain.Models;
using NetCoreReactTempl.Domain.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace NetCoreReactTempl.App.Handlers.Auth.Command
{
    public class Authorization : BaseQuery<AuthInfo, AuthInfo>
    {
        public Authorization(long id, long userId, AuthInfo data)
            : base(id, userId, data) { }
    }

    public class AuthorizationHandler : IRequestHandler<Authorization, AuthInfo>
    {
        private readonly IAuthManager _authManager;
        private readonly IConfigurationStore _configurationStore;

        public AuthorizationHandler(IConfigurationStore configurationStore, IAuthManager authManager)
        {
            _configurationStore = configurationStore;
            _authManager = authManager;
        }

        public async Task<AuthInfo> Handle(Authorization command, CancellationToken cancellationToken)
        {
            var user = await AuthenticateAsync(command.Data.Email, command.Data.Password);

            if (user == null)
                throw new UnauthorizedException(new[] { "Login or password not valid" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configurationStore.AuthSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new System.Security.Claims.Claim[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthInfo()
            {
                Id = user.Id,
                Email = user.Email,
                IsVerify = user.IsVerify,
                Token = tokenString,
                UserId = user.Id
            };
        }

        private async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = (await _authManager.Get(email));

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

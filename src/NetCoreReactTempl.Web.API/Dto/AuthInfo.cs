using NetCoreReactTempl.Web.API.Handlers.Abstractions;

namespace NetCoreReactTempl.Web.API.Dto
{
    public class AuthInfo : BaseDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? IsVerify { get; set; }
        public string Token { get; set; }
    }
}

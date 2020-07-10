namespace NetCoreReactTempl.Domain.Models
{
    public class AuthInfo : BaseData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? IsVerify { get; set; }
        public string Token { get; set; }
    }
}

namespace NetCoreReactTempl.Domain.Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsVerify { get; set; }
        public byte[] ForgotPasswordSalt { get; set; }
        public bool IsAdmin { get; set; }
    }
}

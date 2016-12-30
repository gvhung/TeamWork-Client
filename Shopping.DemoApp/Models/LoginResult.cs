
namespace TeamWork.Models
{
    public class LoginResult
    {
        public string authenticationToken { get; set; }
        public LoginResultUser user { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginResultUser
    {
        public string userId { get; set; }
    }
}

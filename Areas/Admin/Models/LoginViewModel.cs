using NewsApp.Models;

namespace NewsApp.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public Role Role { get; set; }
        public string ReturnUrl { get; set; }
    }
}

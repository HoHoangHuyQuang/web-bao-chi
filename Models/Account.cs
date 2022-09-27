using System.ComponentModel.DataAnnotations;

namespace NewsApp.Models
{
    public class Account
    {
        [Key]
        [Required(ErrorMessage = "Username is required !")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required !")]
        [MaxLength(50, ErrorMessage = "Too long")]
        public string Password { get; set; }

        [Required]
        public int RoleID { get; set; }
        public Role Role { get; set; }


    }
}

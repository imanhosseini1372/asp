using System.ComponentModel.DataAnnotations;

namespace WebShop.Application.Dto.Users
{
    public class LoginDto
    {
        [Required(ErrorMessage = " فیلداجباری")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "فیلداجباری")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

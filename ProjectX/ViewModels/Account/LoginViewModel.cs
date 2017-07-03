using System.ComponentModel.DataAnnotations;

namespace ProjectX.ViewModels.Account
{

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

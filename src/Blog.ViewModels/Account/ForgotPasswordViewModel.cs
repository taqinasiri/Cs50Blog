using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Display(Prompt = "Email")]
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
